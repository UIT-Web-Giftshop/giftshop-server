using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Attributes;
using Domain.Entities;
using Domain.Paging;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features.Coupons.Queries
{
    public class GetPagingCouponQueryHandler : IRequestHandler<GetPagingCouponQuery, ResponseApi<PagingModel<Coupon>>>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ICounterRepository _counterRepository;

        public GetPagingCouponQueryHandler(ICounterRepository counterRepository, ICouponRepository couponRepository)
        {
            _counterRepository = counterRepository;
            _couponRepository = couponRepository;
        }


        public Task<ResponseApi<PagingModel<Coupon>>> Handle(GetPagingCouponQuery request, CancellationToken cancellationToken)
        {
            var (sortDirect, sortField) = PrepareSortDefinition(request);
            var findOptions = PrepareFindOptions(request, sortField, sortDirect);
            var filter = PrepareFilter(request);

            var collectionTarget = BsonCollection.GetCollectionName<Coupon>();
            var totalTask = _counterRepository.FindOneAsync(x => x.CollectionName == collectionTarget, cancellationToken);
            var dataTask = _couponRepository.FindAsync(filter, findOptions, cancellationToken);
            
            Task.WaitAll(new Task[] {totalTask, dataTask}, cancellationToken);

            var dataList = dataTask.Result.ToList(cancellationToken);
            var viewModel = new PagingModel<Coupon>()
            {
                AllTotalCount = totalTask.Result.CurrentCount,
                ItemsCount = dataList.Count,
                Items = dataList
            };
            return Task.FromResult(ResponseApi<PagingModel<Coupon>>.ResponseOk(viewModel));
        }
        
        private static Expression<Func<Coupon, bool>> PrepareFilter(GetPagingCouponQuery request)
        {
            Expression<Func<Coupon, bool>> filter = q => q.IsActive == request.IsActive;
            if (!string.IsNullOrEmpty(request.EventCode))
            {
                filter = q => q.IsActive == request.IsActive && q.EventCode.Equals(request.EventCode);
            }

            return filter;
        }

        private static FindOptions<Coupon, Coupon> PrepareFindOptions(GetPagingCouponQuery request, string sortField, int sortDirect)
        {
            var findOptions = new FindOptions<Coupon, Coupon>
            {
                Limit = request.PagingRequest.PageSize,
                Skip = (request.PagingRequest.PageIndex - 1) * request.PagingRequest.PageSize,
                Sort = new BsonDocument { { sortField, sortDirect } } //-1 giam dan, 1 tang dan
            };
            return findOptions;
        }

        private static (int, string) PrepareSortDefinition(GetPagingCouponQuery request)
        {
            var sortDirect = request.IsDesc ? -1 : 1;
            string sortField = request.SortBy switch
            {
                "percent" => "discountPercent",
                "createdAt" => "createdAt",
                _ => "discountPercent"
            };
            return (sortDirect, sortField);
        }
    }
}