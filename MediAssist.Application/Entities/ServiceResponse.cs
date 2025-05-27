using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Entities
{
    public class ServiceResponse<T>: IServiceResponse<T>
    {
        public bool Success { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public T Data { get; set; }
    }
}
