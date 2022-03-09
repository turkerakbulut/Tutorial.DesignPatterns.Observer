using System;
using System.Collections;
using System.Collections.Generic;

namespace Tutorial.DesignPatterns.Observer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Seller seller = new Seller();

            Product product = new Product()
            {
                ID = 1,
                Name = "The Group Mind Book",
                Price = 100
            };

            Account account1 = new Account("Mustafa");
            Account account2 = new Account("Kemal");
            Account account3 = new Account("Ata");

            account1.RegisterObserver(seller);
            account2.RegisterObserver(seller);
            account3.RegisterObserver(seller);

            seller.NotifyObservers(product);

            Console.Read();
        }
    }

    internal class ObserverCollection : IList<IObserver<Product>>
    {
        private readonly IList<IObserver<Product>> list = new List<IObserver<Product>>();

        public IObserver<Product> this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(IObserver<Product> item)
        {
            if (!list.Contains(item))
                list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(IObserver<Product> item)
        {
            return list.Contains(item);
        }

        public void CopyTo(IObserver<Product>[] array, int arrayIndex)
        {
        }

        public IEnumerator<IObserver<Product>> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(IObserver<Product> item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, IObserver<Product> item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IObserver<Product> item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    internal class Product
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }

    internal class Unsubscriber : IDisposable
    {
        private ObserverCollection observers;
        private IObserver<Product> observer;

        public Unsubscriber(ObserverCollection observers, IObserver<Product> observer)
        {
            this.observers = observers;
            this.observer = observer;
        }

        public void Dispose()
        {
            if (observer != null && observers.Contains(observer))
                observers.Remove(observer);
        }
    }

    internal class Seller : IObservable<Product>
    {
        private ObserverCollection observers;

        public Seller()
        {
            observers = new ObserverCollection();
        }

        public IDisposable Subscribe(IObserver<Product> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        public void NotifyObservers(Product product)
        {
            foreach (var observer in observers)
            {
                if (product == null)
                    observer.OnError(new ApplicationException());
                else
                    observer.OnNext(product);
            }
        }
    }

    internal class Account : IObserver<Product>
    {
        private IDisposable unsubscriber;

        public Account(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public virtual void RegisterObserver(IObservable<Product> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        public void OnCompleted()
        {
            Console.WriteLine("On completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.ToString());
        }

        public void OnNext(Product value)
        {
            Console.WriteLine("Account :{0}, New product: {1}", this.Name, value.Name + ",price: " + value.Price + " TL");
        }

        public virtual void UnregisterObserver()
        {
            unsubscriber.Dispose();
        }
    }
}