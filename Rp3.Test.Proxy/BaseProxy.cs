﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Rp3.Test.Proxies
{
    public abstract class BaseProxy
    {
        public string BaseAdress
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["webApiUrl"];
            }
        }

        

        protected T HttpGet<T>(string uri, params object[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync($"{BaseAdress}{string.Format(uri, args)}").Result;

                response.EnsureSuccessStatusCode();

                var dataObjects = response.Content.ReadAsAsync<T>().Result;
                return dataObjects;
            }
        }

        protected T HttpPostAsJson<T>(string uri, object data)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsJsonAsync($"{BaseAdress}{uri}", data).Result;

                response.EnsureSuccessStatusCode();

                var dataObjects = response.Content.ReadAsAsync<T>().Result;
                return dataObjects;
            }
        }

        /// <summary>
        /// Incorporamos el método Delete para poder eliminar las transacciones
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected T HttpDelete<T>(string uri, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.DeleteAsync($"{BaseAdress}{uri}"+"/"+id).Result;

                response.EnsureSuccessStatusCode();

                var dataObjects = response.Content.ReadAsAsync<T>().Result;
                return dataObjects;
            }
        }
    }
}