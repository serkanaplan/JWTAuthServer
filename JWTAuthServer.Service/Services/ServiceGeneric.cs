﻿using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System.Linq.Expressions;
using JWTAuthServer.Core.Repositories;
using JWTAuthServer.Core.Services;
using JWTAuthServer.Core.UnitOfWork;

namespace JWTAuthServer.Service.Services;
public class ServiceGeneric<TEntity, TDto>(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository) : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly IGenericRepository<TEntity> _genericRepository = genericRepository;

    public async Task<Response<TDto>> AddAsync(TDto entity)
    {
        var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

        await _genericRepository.AddAsync(newEntity);
 
        await _unitOfWork.CommmitAsync();

        var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

        return Response<TDto>.Success(newDto, 200);
    }

    public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
    {
        var products = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());

        return Response<IEnumerable<TDto>>.Success(products, 200);
    }

    public async Task<Response<TDto>> GetByIdAsync(int id)
    {
        var product = await _genericRepository.GetByIdAsync(id);

        if (product == null)
        {
            return Response<TDto>.Fail("Id not found", 404, true);
        }

        return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product), 200);
    }

    public async Task<Response<NoDataDto>> Remove(int id)
    {
        var isExistEntity = await _genericRepository.GetByIdAsync(id);

        if (isExistEntity == null)
        {
            return Response<NoDataDto>.Fail("Id not found", 404, true);
        }

        _genericRepository.Remove(isExistEntity);

        await _unitOfWork.CommmitAsync();
        //204 durum kodu =>  No Content  => Response body'sinde hiç bir dat  olmayacak.
        return Response<NoDataDto>.Success(204);
    }

    public async Task<Response<NoDataDto>> Update(TDto entity, int id)
    {
        var isExistEntity = await _genericRepository.GetByIdAsync(id);

        if (isExistEntity == null)
        {
            return Response<NoDataDto>.Fail("Id not found", 404, true);
        }

        var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

        _genericRepository.Update(updateEntity);

        await _unitOfWork.CommmitAsync();
        return Response<NoDataDto>.Success(204);
    }

    public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        var list = _genericRepository.Where(predicate);

        return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
    }
}