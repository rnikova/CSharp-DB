﻿namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Globalization;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            this.CreateMap<DepartmentDto, Department>();

            this.CreateMap<PrisonerDto, Prisoner>()
                .ForMember(x => x.ReleaseDate, y => y.MapFrom(
                    x => DateTime.ParseExact(x.ReleaseDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(x => x.IncarcerationDate, y => y.MapFrom(
                    x => DateTime.ParseExact(x.IncarcerationDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture)));

            this.CreateMap<OfficerDto, Officer>()
                .ForMember(x => x.Position, y => y.MapFrom(
                    x => Enum.Parse(typeof(Position), x.Position)))
                .ForMember(x => x.Weapon, y => y.MapFrom(
                    x => Enum.Parse(typeof(Weapon), x.Weapon)));
        }
    }
}
