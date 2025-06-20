using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

namespace ApiDocDemo.Controllers;

/// <summary>
/// 测试控制器，演示API版本控制
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")] // 设置路由以包含版本号
[ApiVersion("1.0")] // 声明版本 1.0
[ApiVersion("2.0")] // 声明版本 2.0
[Produces("application/json")]
// 注意：查询字符串、Header和媒体类型版本控制通常不需要额外的路由属性
public class HelloWorldController : ControllerBase
{
    /// <summary>
    /// 获取用户列表（版本1.0）
    /// </summary>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     GET /api/v1.0/HelloWorld
    ///     
    /// </remarks>
    /// <returns>返回用户列表</returns>
    /// <response code="200">成功返回用户列表</response>
    /// <response code="400">请求参数错误</response>
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(object[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetUsers()
    {
        var users = new[] { new { Id = 1, Name = "John Doe" } };
        return Ok(users);
    }

    /// <summary>
    /// 获取用户列表（版本2.0）
    /// </summary>
    /// <remarks>
    /// 示例请求：
    /// 
    ///     GET /api/v2.0/HelloWorld
    ///     
    /// 版本2.0增加了邮箱字段
    /// </remarks>
    /// <returns>返回包含邮箱的用户列表</returns>
    /// <response code="200">成功返回用户列表</response>
    /// <response code="400">请求参数错误</response>
    [HttpGet]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(object[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetUsersV2()
    {
        var users = new[] { new { Id = 1, Name = "John Doe", Email = "john.doe@example.com" } };
        return Ok(users);
    }
}