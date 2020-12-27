using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    [RoutePrefix("api/posts")]
    public class PostController : ApiController
    {
        PostsRepository postsrepository = new PostsRepository();
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(postsrepository.GetAll());
        }
        [Route("{id}", Name = "GetPostById")]
        public IHttpActionResult Get(int id)
        {
            var posts = postsrepository.Get(id);
            if (posts == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            posts.Links.Add(new Link() { Url = HttpContext.Current.Request.Url.AbsoluteUri.ToString(), Method = "GET", Relation = "Self" });
            posts.Links.Add(new Link() { Url = "http://localhost:58618/api/posts", Method = "GET", Relation = "Get All" });
            posts.Links.Add(new Link() { Url = "http://localhost:58618/api/posts", Method = "POST", Relation = "Create" });
            posts.Links.Add(new Link() { Url = HttpContext.Current.Request.Url.AbsoluteUri.ToString(), Method = "PUT", Relation = "Update" });
            posts.Links.Add(new Link() { Url = HttpContext.Current.Request.Url.AbsoluteUri.ToString(), Method = "DELETE", Relation = "Delete" });
            return Ok(postsrepository.Get(id));
        }
        [Route("")]
        public IHttpActionResult Post(post post)
        {
            postsrepository.Insert(post);
            string uri = Url.Link("GetPostById", new { id = post.id });
            return Created(uri, post);
        }
        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id,[FromBody] post post)
        {
            post.id = id;
            postsrepository.Update(post);
            return Ok(post);
        }
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
           postsrepository.Delete(id);
           return StatusCode(HttpStatusCode.NoContent);
        }
    }
}