using System;
using System.Linq.Expressions;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Extensions.Mongo
{
    public static class AggregateFluentExtension
    {
        public static IAggregateFluent<TAggr> UnionWithOther<TAggr, TOther>(
            this IAggregateFluent<TAggr> aggregation,
            IRefactorRepository<TOther> other,
            Expression<Func<TOther, TAggr>> projection) where TOther : class
        {
            var otherImpl = other as RefactorRepository<TOther>;
            var pipelineDefinition = PipelineDefinitionBuilder.For<TOther>().Project(projection);

            return aggregation.UnionWith(otherImpl.Collection, pipelineDefinition);
        }
    }
}