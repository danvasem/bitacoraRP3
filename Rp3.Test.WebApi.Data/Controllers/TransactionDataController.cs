using Rp3.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rp3.Test.WebApi.Data.Controllers
{
    public class TransactionDataController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(int? personId)
        {            
            List<Rp3.Test.Common.Models.TransactionView> commonModel = new List<Common.Models.TransactionView>();

            if (personId.HasValue)
            {

                using (DataService service = new DataService())
                {
                    IEnumerable<Rp3.Test.Data.Models.Transaction>
                        dataModel = service.Transactions.Get(p => p.PersonId == personId,
                        includeProperties: "Category,TransactionType",
                        orderBy: p => p.OrderByDescending(o => o.RegisterDate));

                    //Para incluir una condición, puede usar el primer parametro de Get
                    /*
                     * Ejemplo
                     IEnumerable<Rp3.Test.Data.Models.Transaction>
                        dataModel = service.Transactions.Get(p=> p.TransactionId > 0
                        includeProperties: "Category,TransactionType",
                        orderBy: p => p.OrderByDescending(o => o.RegisterDate));

                     */

                    commonModel = dataModel.Select(p => new Common.Models.TransactionView()
                    {
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
                        Notes = p.Notes,
                        Amount = p.Amount,
                        RegisterDate = p.RegisterDate,
                        ShortDescription = p.ShortDescription,
                        TransactionId = p.TransactionId,
                        TransactionType = p.TransactionType.Name,
                        TransactionTypeId = p.TransactionTypeId
                    }).ToList();
                }
            }

            return Ok(commonModel);
        }

        /// <summary>
        /// Método para obtener el balance de transacciones
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Balance(int? personId)
        {
            List<Rp3.Test.Common.Models.BalanceView> commonModel = new List<Common.Models.BalanceView>();

            if (personId.HasValue)
            {

                using (DataService service = new DataService())
                {
                    IEnumerable<Rp3.Test.Data.Models.Balance>
                        dataModel = service.Balances.GetWithRawSql(string.Format(@"SELECT 
                                C.Name Category,
                                SUM(CASE WHEN T.TransactionTypeId = 2 THEN T.Amount * -1 ELSE T.Amount END) Amount
                            FROM tbTransaction T
                            INNER JOIN tbCategory C ON T.CategoryId = C.CategoryId
                            where PersonId={0}
                            GROUP BY C.Name",personId));

                    commonModel = dataModel.Select(p => new Common.Models.BalanceView()
                    {
                        Category = p.Category,
                        Amount=p.Amount
                    }).ToList();
                }
            }

            return Ok(commonModel);
        }

        /// <summary>
        /// Método para crear un nuevo registro de trnsacción
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Insert(Rp3.Test.Common.Models.Transaction transaction)
        {
            //Complete the code
            using (DataService service = new DataService())
            {
                Rp3.Test.Data.Models.Transaction model = new Test.Data.Models.Transaction();
                model.TransactionId = service.Transactions.GetMaxValue<int>(p => p.TransactionId, 0) + 1;
                model.TransactionTypeId = transaction.TransactionTypeId;
                model.RegisterDate = transaction.RegisterDate;
                model.Notes = transaction.Notes;
                model.ShortDescription = transaction.ShortDescription;
                model.PersonId = transaction.PersonId;
                model.CategoryId = transaction.CategoryId;
                model.Amount = transaction.Amount;


                service.Transactions.Insert(model);
                service.SaveChanges();
            }

            return Ok();
        }

        /// <summary>
        /// Método para actualizar un nuevo registro de transacción
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Update(Rp3.Test.Common.Models.Transaction transaction)
        {
            //Complete the code
            using (DataService service = new DataService())
            {
                Rp3.Test.Data.Models.Transaction model = service.Transactions.GetByID(transaction.TransactionId);
                model.TransactionId = transaction.TransactionId;
                model.TransactionTypeId = transaction.TransactionTypeId;
                model.RegisterDate = transaction.RegisterDate;
                model.Notes = transaction.Notes;
                model.ShortDescription = transaction.ShortDescription;
                model.PersonId = transaction.PersonId;
                model.CategoryId = transaction.CategoryId;
                model.Amount = transaction.Amount;

                service.Transactions.Update(model);
                service.SaveChanges();
            }

            return Ok();
        }

        /// <summary>
        /// Método para eliminar un nuevo registro de transacción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            //Complete the code
            using (DataService service = new DataService())
            {
                Rp3.Test.Data.Models.Transaction model = new Test.Data.Models.Transaction
                {
                    TransactionId = id
                };

                service.Transactions.Delete(model);
                service.SaveChanges();
            }

            return Ok();
        }
    }
}
