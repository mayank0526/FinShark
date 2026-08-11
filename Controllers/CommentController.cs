using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Dtos.Stock.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("api/comment")]
    [ApiController]

    public class CommentController : ControllerBase

    {

        private readonly ICommentRepo comrepo;
        private readonly IStockRepo stkrepo;
        private readonly UserManager<AppUser> UM;

        public CommentController(ICommentRepo commentrepo, IStockRepo stockRepo, UserManager<AppUser> userManager)
        {
            stkrepo = stockRepo;
            comrepo = commentrepo;
            UM = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await comrepo.GetAllAsync();
            var commentDto = comments.Select(s => s.TocommentDto());
            return Ok(commentDto);

        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById([FromRoute] int id)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comm = await comrepo.GetByIdAsync(id);

            if (comm == null)
            {
                return NotFound();
            }

            return Ok(comm.TocommentDto());
        }

        [HttpPost("{id:int}")]

        public async Task<IActionResult> Create([FromRoute] int id, CreateCommentDto comment)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (!await stkrepo.StockExisit(id))
            {
                return BadRequest("Not Exisist");
            }

            var username = User.GetUsername();
            var appUser = await UM.FindByNameAsync(username);

            var commentModel = comment.Createcomment(id);

            commentModel.AppUserId = appUser.Id;
            await comrepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.TocommentDto());

        }

        [HttpPut]
        [Route("{id:int}")]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto UP)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var comment = await comrepo.UpdateAsync(id, UP.CommentUpdate());

            if (comment == null)
            {
                return NotFound("Comment Not Found");
            }

            return Ok(comment.TocommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cm = await comrepo.DeleteAsync(id);

            if (cm == null)
            {
                return NotFound("Does not Exisist");
            }

            return Ok(cm);

        }

    }
}






















