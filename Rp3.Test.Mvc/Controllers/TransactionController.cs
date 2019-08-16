using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rp3.Test.Mvc.Controllers
{
    public class TransactionController : Controller
    {
        /// <summary>
        /// Devuelve el listado de transacciones de la persona seleccionada ordenados de manera descendente según la fecha
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Proxies.Proxy proxy = new Proxies.Proxy();

            List<Rp3.Test.Mvc.Models.TransactionViewModel> model = new List<Models.TransactionViewModel>();

            if (PersonController.PersonId != null)
            {
                var data = proxy.GetTransactions((int)PersonController.PersonId);

                foreach (var item in data)
                {
                    model.Add(new Models.TransactionViewModel()
                    {
                        Amount = item.Amount,
                        CategoryId = item.CategoryId,
                        CategoryName = item.CategoryName,
                        Notes = item.Notes,
                        RegisterDate = item.RegisterDate,
                        ShortDescription = item.ShortDescription,
                        TransactionId = item.TransactionId,
                        TransactionType = item.TransactionType,
                        TransactionTypeId = item.TransactionTypeId
                    });
                }
            }

            ViewBag.Categories = CategoryController.GetCategories();
            ViewBag.TransactionTypes = GetTransactionTypes();
            ViewBag.Transactions = model.OrderByDescending(X=>X.RegisterDate).ToList();

            return View();
        }

        /// <summary>
        /// Devuelve el balance de transacciones de la persona seleccionada.
        /// </summary>
        /// <returns></returns>
        public ActionResult Balance()
        {
            Proxies.Proxy proxy = new Proxies.Proxy();

            List<Common.Models.BalanceView> model = new List<Common.Models.BalanceView>();

            if (PersonController.PersonId != null)
            {
                var data = proxy.GetBalance((int)PersonController.PersonId);

                foreach (var item in data)
                {
                    model.Add(new Common.Models.BalanceView
                    {
                        Category=item.Category,
                        Amount = item.Amount
                    });
                }

                //Procedemos a sumar el total
                decimal total = model.Sum(X => X.Amount);
                if (total > 0)
                    //Ordenamos de mayor a manor
                    model = model.OrderByDescending(X => X.Amount).ToList();
                else
                    //Ordenamos de menor a mayor
                    model = model.OrderBy(X => X.Amount).ToList();

                ViewBag.Total = total;
            }

            return View(model);
        }

        /// <summary>
        /// Permite Actualizar o Insertar un nuevo registro de transacción asociado a la persona seleccionada
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(Rp3.Test.Mvc.Models.TransactionEditModel model)
        {
            if (model != null)
            {
                Proxies.Proxy proxy = new Proxies.Proxy();

                //Validamos si es para crear o actualizar
                if (model.TransactionId > 0)
                {
                    //Actualizar
                    proxy.UpdateTransaction(new Common.Models.Transaction()
                    {
                        TransactionId = model.TransactionId,
                        TransactionTypeId = model.TransactionTypeId,
                        Amount = model.Amount,
                        CategoryId = model.CategoryId,
                        Notes = model.Notes,
                        RegisterDate = model.RegisterDate,
                        ShortDescription = model.ShortDescription,
                        PersonId=PersonController.PersonId.Value
                    });
                }
                else
                {
                    //Crear
                    proxy.InsertTransaction(new Common.Models.Transaction()
                    {
                        TransactionTypeId = model.TransactionTypeId,
                        Amount = model.Amount,
                        CategoryId = model.CategoryId,
                        Notes = model.Notes,
                        RegisterDate = model.RegisterDate,
                        ShortDescription = model.ShortDescription,
                        PersonId = PersonController.PersonId.Value
                    });
                }
            }
            return Index();
        }

        /// <summary>
        /// Permite eliminar un registro de transacción según su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            Proxies.Proxy proxy = new Proxies.Proxy();
            proxy.DeleteTransaction(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Devuelve los tipos de transacción ordenados alfabéticamente de manera ascendente
        /// </summary>
        /// <returns></returns>
        public static List<Common.Models.TransactionType> GetTransactionTypes()
        {
            Proxies.Proxy proxy = new Proxies.Proxy();

            List<Common.Models.TransactionType> model = new List<Common.Models.TransactionType>();

            if (PersonController.PersonId != null)
            {
                var data = proxy.GetTransactionTypes();

                foreach (var item in data)
                {
                    model.Add(new Common.Models.TransactionType()
                    {
                        Name=item.Name,
                        TransactionTypeId=item.TransactionTypeId
                    });
                }
            }

            return model.OrderBy(X=>X.Name).ToList();
        }

    }
}
