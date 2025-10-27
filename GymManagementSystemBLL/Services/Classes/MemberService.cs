using AutoMapper;
using GymManagementSystemBLL.Services.AttachmentService;
using GymManagementSystemBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.MemberViewModels;
using GymManagementSystemDAL.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper   mapper;
        private readonly IAttachmentService attachmentService;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper,IAttachmentService attachmentService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.attachmentService = attachmentService;
        }

        

        public bool CreateMember(CreateMemberViewModel CreatedModel)
        {
            try
            {
                if (IsEmailExists(CreatedModel.Email) || IsPhoneExists(CreatedModel.Phone)) return false;

                var member=mapper.Map<CreateMemberViewModel,Member>(CreatedModel);
                var photoName= attachmentService.Upload("members", CreatedModel.PhotoFile);
                if(string.IsNullOrEmpty(photoName)) return false;

                member.Photo= photoName;

                unitOfWork.GetRepository<Member>().Add(member) ;
               var isCreated=unitOfWork.SaveChanges() >0;
                if (!isCreated) 
                {
                    attachmentService.Delete(photoName, "members");
                    return false;
                }
                return isCreated;

            }
            catch (Exception)
            { 
                return false;
            }
        }

        public bool DeleteMember(int MemberId)
        {
           var member= unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return false;
            var SessionIds= unitOfWork.GetRepository<MemberSession>().GetAll(x=>x.Id==MemberId).Select(x=>x.SessionId);

            var FutureSessions = unitOfWork.GetRepository<Session>().GetAll(
                x => SessionIds.Contains(x.Id) && x.StartDate > DateTime.Now).Any();
             if (FutureSessions) return false;

             var membership= unitOfWork.GetRepository<MemberShip>().GetAll(x=>x.MemberId==MemberId);
           
            try
            {
                if (membership.Any())
                {
                    foreach (var item in membership)
                    {
                        unitOfWork.GetRepository<MemberShip>().Delete(item);
                    }
                }
                unitOfWork.GetRepository<Member>().Delete(member);
                var isDeleted= unitOfWork.SaveChanges()>0;
                if (isDeleted)
                {
                    attachmentService.Delete(member.Photo, "members");
                }
                return isDeleted;
            }
            catch (Exception) { return false; }

        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member= unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null)
            {
                return null; 
            }

            var ViewModel = mapper.Map<Member, MemberViewModel>(member);
            var ActiveMembership = unitOfWork.GetRepository<MemberShip>().GetAll(x=>x.Id == MemberId && x.Status=="Active").FirstOrDefault();
            if(ActiveMembership is not null)
            {
                ViewModel.MembershipStartDate=ActiveMembership.CreatedAt.ToShortDateString();
                ViewModel.MembershipEndDate=ActiveMembership.EndDate.ToShortDateString();
                var plan = unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId);
                ViewModel.PlanName = plan?.Name;
            }
            return ViewModel;


        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null) return null;
         
            var MappedHealthRecord=mapper.Map<HealthRecord, HealthRecordViewModel>(MemberHealthRecord);
            return MappedHealthRecord;
           
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
           var member= unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null) return null;

            var mappedMember=mapper.Map<Member, MemberToUpdateViewModel>(member);
            return mappedMember;
        }

        public IEnumerable<MemberViewModel> GettAll()
        {
            var members = unitOfWork.GetRepository<Member>().GetAll();
            if (members == null || !members.Any()) return [];

            
            var memberViewModels = mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);
            return memberViewModels;
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
        {
            try
            {
               
                var EmailExists=unitOfWork.GetRepository<Member>().GetAll(x=>x.Email==UpdatedMember.Email && x.Id !=Id);

                var PhoneExists = unitOfWork.GetRepository<Member>().GetAll(x => x.PhoneNumber == UpdatedMember.Phone && x.Id != Id);

                if(EmailExists.Any() || PhoneExists.Any()) return false;

                var memberToUpdate = unitOfWork.GetRepository<Member>().GetById(Id);
                if (memberToUpdate == null) return false;
               
                var mappedMember = mapper.Map(UpdatedMember, memberToUpdate);
                unitOfWork.GetRepository<Member>().Update(memberToUpdate);
                return unitOfWork.SaveChanges()>0;

            }
            catch (Exception)
            { 
             return false;
            }
        }


        #region HelperMethod

        private bool IsEmailExists(string Email)
        {
            return unitOfWork.GetRepository<Member>().GetAll(x=>x.Email == Email).Any();
        }

        private bool IsPhoneExists(string Phone)
        {
            return unitOfWork.GetRepository<Member>().GetAll(x => x.PhoneNumber == Phone).Any();
        }

        #endregion
    }
}
