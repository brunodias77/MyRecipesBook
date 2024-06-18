using Microsoft.EntityFrameworkCore.Storage;
using MRB.Domain.Repositories;

namespace MRB.Infra.Data
{
    // Implementa a interface IUnitOfWork para gerenciar transações e salvar mudanças no contexto do EF Core
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        // Construtor recebe o contexto do aplicativo e uma transação opcional
        public UnitOfWork(ApplicationDbContext context, IDbContextTransaction transaction = null)
        {
            _context = context;
            _transaction = transaction;
        }

        // Salva todas as mudanças feitas no contexto do EF Core no banco de dados
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Inicia uma nova transação no banco de dados
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        // Confirma a transação atual no banco de dados
        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Em caso de erro, reverte a transação
                await _transaction.RollbackAsync();
                throw ex;
            }
        }

        // Implementa a interface IDisposable para liberar recursos
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Libera o contexto do EF Core quando Dispose é chamado
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}