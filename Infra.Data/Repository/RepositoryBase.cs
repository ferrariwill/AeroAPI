using Dapper;
using Dominio.Interfaces;
using Infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Repository
{
    public class RepositoryBase<TEntity> : AdoContexto, IRepositoryBase<TEntity> where TEntity : class
    {

        /// <summary>
        /// Listando os dados da entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(TEntity entity, DynamicParameters parameters, string procName)
        {
            using (var con = new SqlConnection(StrConnection))
            {
                con.Open();
                return SqlMapper.Query<TEntity>(con, procName, parameters, commandTimeout: 0, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Listando os dados da entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="usuario"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(TEntity entity, string usuario, string procName)
        {
            using (var con = new SqlConnection(StrConnection))
            {
                con.Open();
                return SqlMapper.Query<TEntity>(con, procName, new { Usuario = usuario }, commandTimeout: 0, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Listando os dados da entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="usuario"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(TEntity entity, string procName)
        {
            using (var con = new SqlConnection(StrConnection))
            {
                con.Open();
                return SqlMapper.Query<TEntity>(con, procName, commandTimeout: 0, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Lista apenas um dados da entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public TEntity Get(TEntity entity, DynamicParameters parameters, string procName)
        {
            using (var con = new SqlConnection(StrConnection))
            {
                con.Open();
                return SqlMapper.Query<TEntity>(con, procName, parameters, commandTimeout: 0, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// Lista apenas um dados da entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="usuario"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public TEntity Get(TEntity entity, string usuario, string procName)
        {
            using (var con = new SqlConnection(StrConnection))
            {
                con.Open();

                return SqlMapper.Query<TEntity>(con, procName, new { Usuario = usuario }, commandTimeout: 0, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// Lista apenas um dados da entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public TEntity Get(TEntity entity, string procName)
        {
            using (var con = new SqlConnection(StrConnection))
            {
                con.Open();

                return SqlMapper.Query<TEntity>(con, procName, commandTimeout: 0, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// Adicionado um novoarquivo na entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public TEntity Add(TEntity entity, DynamicParameters parameters, string procName)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            using (var con = new SqlConnection(StrConnection))
            {
                con.Open();

                using (var trans = con.BeginTransaction())
                {
                    try
                    {
                        entity = SqlMapper.Query<TEntity>(con, procName, parameters, commandTimeout: 0, commandType: CommandType.StoredProcedure, transaction: trans).FirstOrDefault();

                        if (entity != null)
                        {
                            var retorno = entity.GetType().GetProperties();

                            foreach (var item in retorno)
                            {
                                var nomePropriedade = item.Name;
                                var valorPropriedade = entity.GetType().GetProperty(nomePropriedade).GetValue(entity, null);

                                if (nomePropriedade.ToUpper().Equals("CODIGO", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (valorPropriedade.Equals(0))
                                    {
                                        trans.Commit();
                                        return entity;
                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        return entity;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

                return entity;
            }
        }

        /// <summary>
        /// Atualizando os dados de uma entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public TEntity Update(TEntity entity, DynamicParameters parameters, string procName)
        {
            using (var con = new SqlConnection(StrConnection))
            {
                con.Open();

                using (var trans = con.BeginTransaction())
                {
                    try
                    {
                        entity = SqlMapper.Query<TEntity>(con, procName, parameters, commandTimeout: 0, commandType: CommandType.StoredProcedure, transaction: trans).FirstOrDefault();

                        if (entity != null)
                        {
                            var retorno = entity.GetType().GetProperties();

                            foreach (var item in retorno)
                            {
                                var nomePropriedade = item.Name;
                                var valorPropriedade = entity.GetType()
                                    .GetProperty(nomePropriedade)
                                    .GetValue(entity, null);

                                if (nomePropriedade.ToUpper().Equals("CODIGO", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (valorPropriedade.Equals(0))
                                    {
                                        trans.Commit();
                                        return entity;
                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        return entity;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

                return entity;
            }
        }

        /// <summary>
        /// Deletando os dados de uma entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public TEntity Delete(TEntity entity, DynamicParameters parameters, string procName)
        {
            using (var con = new SqlConnection(StrConnection))
            {
                con.Open();

                using (var trans = con.BeginTransaction())
                {
                    try
                    {
                        entity = SqlMapper.Query<TEntity>(con, procName, parameters, commandTimeout: 0, commandType: CommandType.StoredProcedure, transaction: trans).FirstOrDefault();

                        if (entity != null)
                        {
                            var retorno = entity.GetType().GetProperties();
                            foreach (var item in retorno)
                            {
                                var nomePropriedade = item.Name;
                                var valorPropriedade = entity.GetType()
                                    .GetProperty(nomePropriedade)
                                    .GetValue(entity, null);

                                if (nomePropriedade.ToUpper().Equals("CODIGO", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (valorPropriedade.Equals(0))
                                    {
                                        trans.Commit();
                                        return entity;
                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        return entity;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

                return entity;
            }
        }
    }
}
