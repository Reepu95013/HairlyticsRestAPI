using AutoMapper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.Services
{
    public class HelperService : IHelperService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public HelperService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<DashboardDto> GetDashboardDataCounts()
        {
          var dashboard =   await _userRepository.IGetDashboardAsync();

            return _mapper.Map<DashboardDto>(dashboard);
        }
    }
}
