using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        /// <summary>
        /// Listando os dados de uma entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(TEntity entity, DynamicParameters parameters, string procName);

        /// <summary>
        /// Listando os dados de uma entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="usuario"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(TEntity entity, string usuario, string procName);

        /// <summary>
        /// Listando os dados de uma entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(TEntity entity, string procName);

        /// <summary>
        /// Listando os dados de uma entidade
        /// </summary
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        TEntity Get(TEntity entity, DynamicParameters parameters, string procName);

        /// <summary>
        /// Listando os dados de uma entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="usuario"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        TEntity Get(TEntity entity, string usuario, string procName);

        /// <summary>
        /// Listando os dados de uma entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        TEntity Get(TEntity entity, string procName);

        /// <summary>
        /// Adicionando uma nova entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity, DynamicParameters parameters, string procName);

        /// <summary>
        /// Atualizando uma nova entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        TEntity Update(TEntity entity, DynamicParameters parameters, string procName);

        /// <summary>
        /// Deletando os dados de uma entidade
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameters"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        TEntity Delete(TEntity entity, DynamicParameters parameters, string procName);
    }
}
