using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace MatchGame.Models
{
    public class AutoMapperProfile:Profile
    {
        public static IMapper _mapper;
        public AutoMapperProfile(IMapper mapper) {
            CreateMap<Book, BookView>().ReverseMap();

            _mapper = mapper;
            //var person = _mapper.Map<Person>(student);
        }
    }
}