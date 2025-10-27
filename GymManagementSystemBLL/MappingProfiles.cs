using AutoMapper;
using GymManagementSystemBLL.ViewModels.MembershipViewModels;
using GymManagementSystemBLL.ViewModels.MemberViewModels;
using GymManagementSystemBLL.ViewModels.PlanViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.TrainerViewModels;
using GymManagementSystemDAL.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Sessions
            MapSessions();
            #endregion


            #region Trainers
            MapTrainers();

            #endregion

            #region Members

            MapMembers();


            #endregion


            #region Plans

            MapPlans();

            #endregion

            MapMemberships();
        }
        private void MapSessions()
        {
            CreateMap<Session, SessionViewModel>()
                 .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.Category.CategoryName))
                 .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.Trainer.Name))
                 .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, Options => Options.MapFrom(src => src.CategoryName));


        }

        private void MapTrainers()
        {
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Phone, Options => Options.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.DateOfBirth, Options => Options.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Specialization, Options => Options.MapFrom(src => src.Specialties.ToString()))
                  .ForMember(dest => dest.Address,
                  Options => Options.MapFrom(
                      src => $"{src.Address.BuildingNumber} . {src.Address.Street} . {src.Address.City}"));

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.PhoneNumber, Options => Options.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Address, Options => Options.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));
            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Specialization, Options => Options.MapFrom(src => src.Specialties))
                .ForMember(dest => dest.BuildingNumber, Options => Options.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, Options => Options.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, Options => Options.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Phone, Options => Options.MapFrom(src => src.PhoneNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForMember(dest => dest.Name, Options => Options.Ignore())
                .ForMember(dest => dest.Specialties, Options => Options.MapFrom(src => src.Specialization))
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.Now;
                })
                .ForMember(dest => dest.PhoneNumber, Options => Options.MapFrom(src => src.Phone));

        }

        private void MapMembers()
        {
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, Options => Options.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Gender, Options => Options.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                 .ForMember(dest => dest.Address,
                  Options => Options.MapFrom(
                      src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                 .ForMember(dest => dest.Phone, Options => Options.MapFrom(src => src.PhoneNumber));





            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, Options => Options.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City

                }))
                .ForMember(dest => dest.HealthRecord, Options => Options.MapFrom(src => src.HealthRecordViewModel))
                 .ForMember(dest => dest.PhoneNumber, Options => Options.MapFrom(src => src.Phone));



            CreateMap<HealthRecordViewModel, HealthRecord>()
                .ForMember(dest => dest.Height, Options => Options.MapFrom(src => src.Height))
                .ForMember(dest => dest.Weight, Options => Options.MapFrom(src => src.Weight))
                .ForMember(dest => dest.BloodType, Options => Options.MapFrom(src => src.BloodType))
                .ForMember(dest => dest.Note, Options => Options.MapFrom(src => src.Note))
                .ReverseMap();


            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, Options => Options.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, Options => Options.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, Options => Options.MapFrom(src => src.Address.City))
                 .ForMember(dest => dest.Phone, Options => Options.MapFrom(src => src.PhoneNumber));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, Options => Options.Ignore())
                .ForMember(dest => dest.Photo, Options => Options.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.Now;
                })
                 .ForMember(dest => dest.PhoneNumber, Options => Options.MapFrom(src => src.Phone));
        }

        private void MapPlans()
        {
            CreateMap<Plan, PlanViewModel>()
                .ForMember(dest => dest.PlanName, Options => Options.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, Options => Options.Ignore())
                .ForMember(dest => dest.UpdatedAt, Options => Options.MapFrom(src => DateTime.Now));

        }

        private void MapMemberships()
        {

            CreateMap<MemberShip, MembershipForMemberViewModel>()
                     .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                     .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
                     .ForMember(dist => dist.StartDate, Option => Option.MapFrom(X => X.CreatedAt));

            CreateMap<MemberShip, MembershipViewModel>()
                     .ForMember(dist => dist.MemberName, Option => Option.MapFrom(Src => Src.Member.Name))
                     .ForMember(dist => dist.PlanName, Option => Option.MapFrom(Src => Src.Plan.Name))
                                          .ForMember(dist => dist.StartDate, Option => Option.MapFrom(X => X.CreatedAt));

            CreateMap<CreateMembershipViewModel, MemberShip>();
            CreateMap<Member, MemberSelectListViewModel>();
            CreateMap<Plan, PlanSelectListViewModel>();
        }
    }
}
