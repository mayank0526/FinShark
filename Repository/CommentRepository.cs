using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Models;

namespace api.Repository
{
    public class CommentRepository : ICommentRepo
    {
        private readonly ApplicationDBContext con;
        public CommentRepository(ApplicationDBContext context)
        {
                 con = context;   
        }

        public async Task<Comment> CreateAsync(Comment CM)
        {
            await con.Comments.AddAsync(CM);
            await con.SaveChangesAsync();
            return CM;

        }

        public async Task <Comment?> DeleteAsync(int id)
        {
            var comment = await con.Comments.FirstOrDefaultAsync( x=> x.Id == id);

            if (comment == null){
                return null;
            }
            con.Comments.Remove(comment);
            await con.SaveChangesAsync();
            return comment; 
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await con.Comments.Include(a=> a.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await con.Comments.Include(a=> a.AppUser).FirstOrDefaultAsync(c=> c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment CM)
        {
           var Ecomment = await con.Comments.FindAsync(id);
           if (Ecomment == null)
            {
                return null;
            }

            Ecomment.Title  = CM.Title;
            Ecomment.Content = CM.Content;

            await con.SaveChangesAsync();
             return Ecomment;

        }
    }
}