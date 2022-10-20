using MvcProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Bll.Services.Concrete
{
    public class SelectBuilder<T> where T: class, new()
    {
        BaseService<T> _service;
        protected IQueryable<T> _query;

        int _pageSize;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if(value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    _pageSize = value;
                }
            }
        }

        public int Length => _service.GetMany().Count();


        internal SelectBuilder(BaseService<T> service)
        {
            _service = service;
            _query = _service.GetMany();
        }

        protected SelectBuilder(BaseService<T> service, IQueryable<T> query)
        {
            _service = service;
            _query = query;
        }

        public SelectBuilder<T> Page(int number)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            _query = _query.Skip((number - 1) * PageSize).Take(PageSize);
            return this;
        }

        public IEnumerable<TResult> Build<TResult>()
        {
            return AutoMapper.Mapper.Map<IQueryable<T>, IEnumerable<TResult>>(_query);
        }
    }
}
