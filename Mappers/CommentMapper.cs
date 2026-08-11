using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Dtos.Stock.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto TocommentDto(this Comment CM)
        {
            return new CommentDto
            {
                Id = CM.Id,
                Title = CM.Title,
                Content = CM.Content,
                CreatedOn = CM.CreatedOn,
                CreatedBy = CM.AppUser.UserName,
                StockId = CM.StockId

            };
        }

         public static Comment Createcomment(this CreateCommentDto CM, int stockId)
        {
            return new Comment
            {  
                Title = CM.Title,
                Content = CM.Content,
                StockId = stockId
            };
    }

    public static Comment CommentUpdate(this UpdateCommentRequestDto CM)
        {
            return new Comment
            {  
                Title = CM.Title,
                Content = CM.Content,
                
            };
    
    }
}
}
