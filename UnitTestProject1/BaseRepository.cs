using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;

namespace UnitTestProject1
{
    public class BaseRepository<T> where T : class
    {
        protected internal NHibernate.ISession _session;

        public BaseRepository(NHibernate.ISession session)
        {
            _session = session;
        }

        public NHibernate.ISession Session
        {
            set { _session = value; }
        }

        public virtual T GetById(object id)
        {
            return _session.Get<T>(id);
        }


        public virtual T Load(object id)
        {
            return _session.Load<T>(id);
        }

        public virtual IList<T> GetAll()
        {
            return _session.CreateCriteria(typeof(T)).List<T>();
        }

        public virtual IList<T> GetAll(string entityName)
        {
            return _session.CreateCriteria(entityName).List<T>();
        }

        protected internal IList<T> Find(DetachedCriteria dc)
        {
            return dc.GetExecutableCriteria(_session).List<T>();
        }

        protected internal T FindOn(DetachedCriteria dc)
        {
            return dc.GetExecutableCriteria(_session).UniqueResult<T>();
        }

        public virtual void Update(T objectToUpdate)
        {
            _session.Update(objectToUpdate);
        }

        public virtual object Add(T objectToAdd)
        {
            object newId = _session.Save(objectToAdd);
            return newId;
        }
    }
}
