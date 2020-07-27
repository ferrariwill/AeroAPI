using Dapper;
using Dominio.Entidades;
using Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AeroAPI.Controllers
{
    [RoutePrefix("api/passageiro")]
    //[AllowAnonymous]
    public class PassageiroController : ApiController
    {
        [HttpGet]
        [Route("Consultar")]
        public dynamic Consultar(int? Id = null, string Nome = null, string Celular = null)
        {
            try
            {
                PassageiroRepository repository = new PassageiroRepository();
                List<Passageiro> listaPassageiros = new List<Passageiro>();

                var parameters = new DynamicParameters();

                if (Id == null &&
                    string.IsNullOrEmpty(Nome) &&
                    string.IsNullOrEmpty(Celular))
                {
                    throw new Exception("É necessário informar algum dado para pesquisa");
                }

                if (Id != null)
                {
                    parameters.Add("@Id", Id);
                }

                if (!string.IsNullOrEmpty(Nome))
                {
                    parameters.Add("@Nome", Nome);
                }

                if (!string.IsNullOrEmpty(Celular))
                {
                    parameters.Add("@Celular", Celular);
                }

                listaPassageiros = repository.GetAll(new Passageiro(), parameters, Passageiro.PassageiroConProc).ToList();
                return listaPassageiros;
            }
            catch (Exception e)
            {
                return new { codigo = 1, descricao = e.Message };
            }
        }

        [HttpPost]
        [Route("Cadastrar")]
        public dynamic Cadastrar(HttpRequestMessage request)
        {
            try
            {
                string jsonContent = request.Content.ReadAsStringAsync().Result;
                var body = Newtonsoft.Json.Linq.JObject.Parse(jsonContent);

                int Idade;
                string Nome = "";
                string Celular = "";

                if (body["Idade"] == null)
                {
                    throw new Exception("Favor informar uma Idade");
                }
                else
                {
                    if (!int.TryParse(body["Idade"].ToString(),out Idade))
                    {
                        throw new Exception("Favor informar uma Idade Válida");
                    }
                }

                if (body["Nome"] == null)
                {
                    throw new Exception("Favor informar um Nome");
                }
                else
                {
                    Nome = body["Nome"].ToString();
                }

                if (body["Celular"] == null)
                {
                    throw new Exception("Favor informar um Celular");
                }
                else
                {
                    Celular = body["Celular"].ToString();
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Idade", Idade);
                parameters.Add("@Nome", Nome);
                parameters.Add("@Celular", Celular);

                PassageiroRepository repository = new PassageiroRepository();
                Passageiro passageiro = new Passageiro();

                passageiro = repository.Add(new Passageiro(), parameters, Passageiro.PassageiroInsProc);

                return passageiro;
            }
            catch (Exception e)
            {
                return new { codigo = 1, descricao = e.Message };
            }
        }

        [HttpPut]
        [Route("Atualizar")]
        public dynamic Atualizar(HttpRequestMessage request)
        {
            try
            {
                string jsonContent = request.Content.ReadAsStringAsync().Result;
                var body = Newtonsoft.Json.Linq.JObject.Parse(jsonContent);
                int Id;
                int Idade = -1;
                string Nome = "";
                string Celular = "";

                if (body["Id"] == null || !int.TryParse(body["Id"].ToString(),out Id))
                {
                    throw new Exception("É necessário informar um Id de passageiro");
                }

                if (body["Idade"] != null)
                {
                    if (!int.TryParse(body["Idade"].ToString(), out Idade))
                    {
                        throw new Exception("Favor informar uma Idade Válida");
                    }
                }

                if (body["Nome"] != null)
                {
                    Nome = body["Nome"].ToString();
                }

                if (body["Celular"]!= null)
                {
                    Celular = body["Celular"].ToString();
                }       

                if (Idade < 0 &&
                    string.IsNullOrEmpty(Nome) &&
                    string.IsNullOrEmpty(Celular))
                {
                    throw new Exception("É necessário informar algum dado para alterar");
                }


                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);

                if (!string.IsNullOrEmpty(Nome))
                {
                    parameters.Add("@Nome", Nome);
                }

                if (Idade >= 0)
                {
                    parameters.Add("@Idade", Idade);
                }

                if (!string.IsNullOrEmpty(Celular))
                {
                    parameters.Add("@Celular", Celular);
                }

                PassageiroRepository repository = new PassageiroRepository();
                Passageiro passageiro = new Passageiro();

                passageiro = repository.Update(new Passageiro(), parameters, Passageiro.PassageiroAltProc);
                return passageiro;
            }
            catch (Exception e)
            {
                return new { codigo = 1, descricao = e.Message };
            }
        }

        [HttpDelete]
        [Route("Remover")]
        public dynamic Remover(HttpRequestMessage request)
        {
            try
            {
                string jsonContent = request.Content.ReadAsStringAsync().Result;
                var body = Newtonsoft.Json.Linq.JObject.Parse(jsonContent);
                int Id;

                if(body["Id"] == null || !int.TryParse(body["Id"].ToString(), out Id))
                {
                    throw new Exception("É necessário informar um Id de passageiro");
                }

                PassageiroRepository repository = new PassageiroRepository();
                Passageiro passageiro = new Passageiro();
                var parameters = new DynamicParameters();

                parameters.Add("@Id",Id);
                passageiro = repository.Delete(new Passageiro(), parameters, Passageiro.PassageiroDelProc);
                return passageiro;
            }
            catch (Exception e)
            {
                return new { codigo = 1, descricao = e.Message };
            }
        }
    }
}