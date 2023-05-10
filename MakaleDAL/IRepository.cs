using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDAL
{
    public interface IRepository <T> 
        //T tip entitylere karşılık geliyor
        //interfacei bir classa verince buradaki metotları kullanmak zorunda kalacak
        //interfacede metodların sadece isimleri yer alır buradaki sadece imzalarıdır bodyleri yoktur ve geriye dönüş tipini yazacağız
    {
        int Insert(T nesne);
        int Update(T nesne);
        int Delete(T nesne);

        List<T> Liste();
        List<T> Liste(Expression<Func<T,bool>> x); //koşul expression göndereceğimiz için bu şekilde yazdık. [expression: (x => şeklinde yazdığımız ifadeler)]
        T Find(Expression<Func<T, bool>> kosul);

        IQueryable<T> ListQueryable();

    }
}
