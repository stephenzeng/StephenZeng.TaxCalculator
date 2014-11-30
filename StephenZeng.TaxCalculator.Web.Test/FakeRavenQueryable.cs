using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Client.Spatial;
using Raven.Json.Linq;

namespace StephenZeng.TaxCalculator.Web.Test
{
    public class FakeRavenQueryable<T> : IRavenQueryable<T>
    {
        private IQueryable<T> source;

        public RavenQueryStatistics QueryStatistics { get; set; }

        public FakeRavenQueryable(IQueryable<T> source, RavenQueryStatistics stats = null)
        {
            this.source = source;
            QueryStatistics = stats;
        }

        public IRavenQueryable<T> Customize(Action<IDocumentQueryCustomization> action)
        {
            return this;
        }

        public IRavenQueryable<TResult> TransformWith<TTransformer, TResult>() where TTransformer : AbstractTransformerCreationTask, new()
        {
            throw new NotImplementedException();
        }

        public IRavenQueryable<TResult> TransformWith<TResult>(string transformerName)
        {
            throw new NotImplementedException();
        }

        public IRavenQueryable<T> AddQueryInput(string name, RavenJToken value)
        {
            throw new NotImplementedException();
        }

        public IRavenQueryable<T> AddTransformerParameter(string name, RavenJToken value)
        {
            throw new NotImplementedException();
        }

        public IRavenQueryable<T> Spatial(Expression<Func<T, object>> path, Func<SpatialCriteriaFactory, SpatialCriteria> clause)
        {
            throw new NotImplementedException();
        }

        public IRavenQueryable<T> Statistics(out RavenQueryStatistics stats)
        {
            stats = QueryStatistics;
            return this;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return source.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return source.GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return source.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return new FakeRavenQueryProvider(source, QueryStatistics); }
        }
    }

    public class FakeRavenQueryProvider : IQueryProvider
    {
        private readonly IQueryable _source;
        private readonly RavenQueryStatistics _stats;

        public FakeRavenQueryProvider(IQueryable source, RavenQueryStatistics stats = null)
        {
            this._source = source;
            this._stats = stats;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new FakeRavenQueryable<TElement>(_source.Provider.CreateQuery<TElement>(expression), _stats);
        }

        public IQueryable CreateQuery(Expression expression)
        {

            var type = typeof(FakeRavenQueryable<>).MakeGenericType(expression.Type);
            return (IQueryable)Activator.CreateInstance(type, _source.Provider.CreateQuery(expression), _stats);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _source.Provider.Execute<TResult>(expression);
        }

        public object Execute(Expression expression)
        {
            return _source.Provider.Execute(expression);
        }
    }
}
