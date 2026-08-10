using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepo
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment ?>GetByIdAsync(int id);
        Task<Comment> CreateAsync (Comment CM); 
        Task<Comment?> UpdateAsync (int id, Comment CM );
       
       Task<Comment?> DeleteAsync (int id );
    }

}
