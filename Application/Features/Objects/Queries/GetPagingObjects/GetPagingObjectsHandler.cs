using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Attributes;
using Domain.Entities;
using Domain.Paging;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Objects.Queries.GetPagingObjects
{
    public abstract class GetPagingObjectsHandler<T, V> : Handler<T> where T : class where V : class
    {
        private readonly ISaveFlagRepository _saveFlagRepository;

        public GetPagingObjectsHandler(IBaseRepository<T> _baseRepository, IMapper _mapper, 
            ISaveFlagRepository _saveFlagRepository) : base(_baseRepository, _mapper)
        {
            this._saveFlagRepository = _saveFlagRepository;
        }

        public virtual Expression<Func<T, bool>> GetExpression(GetPagingObjectsQuery request)
        {
            return null;
        }

        public virtual Expression<Func<T, object>> GetSortExpression(GetPagingObjectsQuery request)
        {
            return null;
        }

        protected async Task<ResponseApi<PagingModel<V>>> Handle(GetPagingObjectsQuery request,
            CancellationToken cancellationToken)
        {
            var expression = GetExpression(request);
            var sortExpression = GetSortExpression(request);
            var dataList = await this._baseRepository.GetPagingAsync( new PagingRequest() 
                { PageIndex = request.PageIndex, PageSize = request.PageSize }, expression, 
                sortExpression, request.IsSortAscending, cancellationToken);

            // check if there's not anything in collection
            if (dataList.Any() == false)
            {
                return ResponseApi<PagingModel<V>>.ResponseOk( new PagingModel<V>() { AllTotalCount = 0, 
                    ItemsCount = 0, Items = null });
            }

            // Attempt to get total count of products
            SaveFlag saveFlag;

            try
            {
                saveFlag = await this._saveFlagRepository.GetOneAsync( 
                    x => x.CollectionName == BsonCollection.GetCollectionName<T>(), cancellationToken);
            }
            catch (Exception)
            {
                await this._saveFlagRepository.AddAsync(new SaveFlag() {
                    CollectionName = BsonCollection.GetCollectionName<T>(), 
                    CurrentCount = dataList.Count() }, cancellationToken);

                saveFlag = await this._saveFlagRepository.GetOneAsync(
                    x => x.CollectionName == BsonCollection.GetCollectionName<T>(), cancellationToken);
            }

            var data = this._mapper.Map<List<V>>(dataList);

            return ResponseApi<PagingModel<V>>.ResponseOk(new PagingModel<V> { AllTotalCount = 
                saveFlag.CurrentCount, ItemsCount = dataList.Count(), Items = data });
        }
    }
}