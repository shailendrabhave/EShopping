using MediatR;
using Ordering.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Queries
{
    public class GetOrdersQuery:IRequest<List<OrderReponse>>
    {
       public GetOrdersQuery(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}
