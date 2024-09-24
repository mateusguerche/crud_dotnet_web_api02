﻿namespace WebAPI_Projeto02.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }

        void Commit();
    }
}
