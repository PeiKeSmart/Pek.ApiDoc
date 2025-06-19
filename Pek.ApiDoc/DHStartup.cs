using System.Reflection;

using Asp.Versioning;

using Pek.Infrastructure;
using Pek.VirtualFileSystem;

using Scalar.AspNetCore;

namespace Pek.ApiDoc;

/// <summary>
/// 表示应用程序启动时配置服务和中间件的对象
/// </summary>
public class DHStartup : IPekStartup
{
    /// <summary>
    /// 配置添加的中间件的使用
    /// </summary>
    /// <param name="application"></param>
    public void Configure(IApplicationBuilder application)
    {

    }

    /// <summary>
    /// 将区域路由写入数据库
    /// </summary>
    public void ConfigureArea()
    {
        //AreaBase.SetRoute<HomeController>(AdminArea.AreaName);
    }

    /// <summary>
    /// 添加并配置任何中间件
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="webHostEnvironment"></param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment? webHostEnvironment)
    {
        // 添加 API Explorer，以提供可用版本的信息
        services.AddEndpointsApiExplorer();
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0); // 默认 API 版本（v1.0）
            options.AssumeDefaultVersionWhenUnspecified = true; // 未指定时假设默认版本
            options.ReportApiVersions = true; // 在响应头中报告 API 版本

            // 结合多种版本控制方式
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("version"),
                new UrlSegmentApiVersionReader(),   // 使用 URL 段版本（例如，/api/v1/resource）
                new HeaderApiVersionReader("X-API-Version"),
                new MediaTypeApiVersionReader("version")
            );
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV"; // 版本分组格式
            options.SubstituteApiVersionInUrl = true; // 在 URL 中替换 API 版本
        });

        // 启用 OpenAPI 文档支持，并配置 OpenAPI 文档。
        services.AddOpenApi(opt =>
        {
            // 配置 XML 注释支持
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                opt.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    // .NET 9 的 OpenAPI 会自动处理 XML 注释
                    return Task.CompletedTask;
                });
            }
            //// 添加一个文档转换器，用于处理特定的安全方案（例如: Bearer Token）。
            //opt.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
    }

    /// <summary>
    /// 注册路由
    /// </summary>
    /// <param name="endpoints">路由生成器</param>
    public void UseDHEndpoints(IEndpointRouteBuilder endpoints)
    {
        // 映射 OpenAPI 文档路由。
        endpoints.MapOpenApi();

        // 映射自定义的 API 文档路由，使用 /help 路径
        endpoints.MapScalarApiReference("/help", options =>
        {
            options
            .WithTitle("登灏系统API")
            .WithTheme(ScalarTheme.Mars)
            .WithSidebar(true)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }

    /// <summary>
    /// 配置虚拟文件系统
    /// </summary>
    /// <param name="options"></param>
    public void ConfigureVirtualFileSystem(DHVirtualFileSystemOptions options)
    {
        options.FileSets.AddEmbedded<DHStartup>(typeof(DHStartup).Namespace);
        // options.FileSets.Add(new EmbeddedFileSet(item.Assembly, item.Namespace));
    }

    /// <summary>
    /// 调整菜单
    /// </summary>
    public void ChangeMenu()
    {

    }

    /// <summary>
    /// 升级处理逻辑
    /// </summary>
    public void Update()
    {

    }

    /// <summary>
    /// 配置使用添加的中间件
    /// </summary>
    /// <param name="application">用于配置应用程序的请求管道的生成器</param>
    public void ConfigureMiddleware(IApplicationBuilder application)
    {

    }

    /// <summary>
    /// UseRouting前执行的数据
    /// </summary>
    /// <param name="application"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void BeforeRouting(IApplicationBuilder application)
    {

    }

    /// <summary>
    /// UseAuthentication或者UseAuthorization后面 Endpoints前执行的数据
    /// </summary>
    /// <param name="application"></param>
    public void AfterAuth(IApplicationBuilder application)
    {

    }

    /// <summary>
    /// 处理数据
    /// </summary>
    public void ProcessData()
    {
    }

    /// <summary>
    /// 获取此启动配置实现的顺序
    /// </summary>
    public Int32 StartupOrder => 101; //常见服务应在错误处理程序之后加载

    /// <summary>
    /// 获取此启动配置实现的顺序。主要针对ConfigureMiddleware、UseRouting前执行的数据、UseAuthentication或者UseAuthorization后面 Endpoints前执行的数据
    /// </summary>
    public Int32 ConfigureOrder => 0;
}
