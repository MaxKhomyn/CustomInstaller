using System;
using System.Collections.Generic;
using System.IO;

namespace WIXInstaller.BLL.Model.Iterator
{
    public interface IIterator
    {
        object Current { get; }
        int position { get; set; }

        void Reset();
        void ResetToEnd();
        bool MoveNext();
        bool MovePrev();
    }

    public class Iterator : IIterator
    {
        public List<object> collection;
        public int position { get; set; } = -1; 

        public Iterator()
        {
            collection = new List<object>();
        }
        public Iterator(List<object> collection)
        {
            this.collection = collection;
        }
        public void Reset()
        {
            position = -1;
        }
        
        public void ResetToEnd()
        {
            position = collection.Count;
        }
        public bool MoveNext()
        {
            position++;
            return (position < collection.Count);
        }
        public bool MovePrev()
        {
            position--;
            return (position > -1);
        }
        public object Current
        {
            get
            {
                try
                {
                    return collection[position];
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new InvalidOperationException("Кінець!");
                }
            }
        }

        public void AddElement(object Element)
        {
            collection.Add(Element);
        }

        public void RemoveElement(object Element)
        {
            collection.Remove(Element);
        }
        public void RemoveElement(int Index)
        {
            collection.RemoveAt(Index);
        }

        public void EditElement(int Index, object Element)
        {
            collection.RemoveAt(Index);
            collection.Insert(Index, Element);
        }
    }
}