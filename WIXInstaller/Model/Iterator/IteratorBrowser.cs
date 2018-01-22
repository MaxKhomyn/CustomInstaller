using System.Collections.Generic;

namespace WIXInstaller.BLL.Model.Iterator
{
    /// <summary>
    /// Клас, що працює з ітератором
    /// </summary>
    public class IteratorBrowser
    {
        public IIterator iterator { get; set; }

        public IteratorBrowser(IIterator iterator = null)
        {
            if (iterator != null)
                this.iterator = iterator;
            else this.iterator = new Iterator();
        }

        /// <summary>
        /// Повертає весь список елементів ітератора
        /// </summary>
        public ICollection<object> GetAll()
        {
            ICollection<object> list = new List<object>();

            iterator.Reset();        
            while (iterator.MoveNext())
                list.Add(iterator.Current);
            iterator.Reset();
            iterator.MoveNext();
            return list;    
        }

        /// <summary>
        /// Повертає наступний елемент ітератора
        /// </summary>
        public object GetNext()
        {
            if (iterator.MoveNext())
                return iterator.Current;
            else
            {
                iterator.Reset();
                iterator.MoveNext();
                return iterator.Current;
            }
        }

        /// <summary>
        /// Повертає попередній елемент ітератора
        /// </summary>
        public object GetPrev()
        {
            if (iterator.MovePrev())
                return iterator.Current;
            else
            {
                iterator.ResetToEnd();
                iterator.MovePrev();
                return iterator.Current;
            }                
        }

        public object GetFirst()
        {
            iterator.Reset();
            iterator.MoveNext();
            return iterator.Current;
        }
        public object GetLast()
        {
            iterator.ResetToEnd();
            iterator.MovePrev();
            return iterator.Current;
        }
        
        public int GetPosition()
        {
            return iterator.position;
        }
    }
}