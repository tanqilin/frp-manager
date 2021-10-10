using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Frp.NetWork.Model
{
    /// <summary>
    /// FRPS配置信息
    /// </summary>
    public class FrpsConfig
    {
        /// <summary>
        /// 基础配置
        /// </summary>
        public StartConfig startConfig {  get; set; } = new StartConfig();

        /// <summary>
        /// 权限配置
        /// </summary>
        public AuthenConfig authenConfig {  get; set; } = new AuthenConfig();

        /// <summary>
        /// 管理配置
        /// </summary>
        public AdminConfig adminConfig { get; set; } = new AdminConfig();

        /// <summary>
        /// Dashboard 管理监控
        /// </summary>
        public DashboardConfig dashboardConfig { get; set; } = new DashboardConfig();

        /// <summary>
        /// HTTP & HTTPS
        /// </summary>
        public HttpConfig httpConfig { get; set; } = new HttpConfig();
    }

    /// <summary>
    /// 基础配置
    /// </summary>
    public class StartConfig
    {
        /// <summary>
        /// 服务端监听地址
        /// </summary>
        [DefaultValue("0.0.0.0")]
        public string bind_addr { get; set; } = "0.0.0.0";

        /// <summary>
        /// 监听端口
        /// </summary>
        public int bind_port { get; set; } = 7000;

        /// <summary>
        /// 服务端监听 UDP 端口
        /// </summary>
        public int bind_udp_port { get; set; }

        /// <summary>
        /// 服务端监听 KCP 协议端口
        /// </summary>
        public int kcp_bind_port { get; set; }

        /// <summary>
        /// 代理监听地址
        /// </summary>
        public string proxy_bind_addr { get; set; }

        /// <summary>
        /// 日志文件
        /// </summary>
        [DefaultValue("./frps.log")]
        public string log_file { get; set; } = "./frps.log";

        /// <summary>
        /// 日志等级
        /// </summary>
        [DefaultValue("info")]
        public string log_level { get; set; } = "info";

        /// <summary>
        /// 日志文件保留天数
        /// </summary>
        [DefaultValue(3)]
        public int log_max_days { get; set; } = 3;

        /// <summary>
        /// 禁用标准输出中的日志颜色
        /// </summary>
        [DefaultValue(false)]
        public bool disable_log_color { get; set; }

        /// <summary>
        /// 服务端返回详细错误信息给客户端
        /// </summary>
        [DefaultValue(false)]
        public bool detailed_errors_to_client { get; set; }

        /// <summary>
        /// 服务端和客户端心跳连接的超时时间
        /// </summary>
        [DefaultValue(90)]
        public int heart_beat_timeout { get; set; } = 90;

        /// <summary>
        /// 用户建立连接后等待客户端响应的超时时间
        /// </summary>
        [DefaultValue(10)]
        public int user_conn_timeout { get; set; } = 10;

        /// <summary>
        /// 代理 UDP 服务时支持的最大包长度
        /// </summary>
        [DefaultValue(1500)]
        public int udp_packet_size { get; set; } = 1500;

        /// <summary>
        /// TLS 服务端证书文件路径
        /// </summary>
        public string tls_cert_file { get; set; }

        /// <summary>
        /// TLS 服务端密钥文件路径
        /// </summary>
        public string tls_key_file { get; set; }

        /// <summary>
        /// TLS CA 证书路径
        /// </summary>
        public string tls_trusted_ca_file { get; set; }
    }

    /// <summary>
    /// 权限配置
    /// </summary>
    public class AuthenConfig
    {
        /// <summary>
        /// 鉴权方式:token(默认), oidc
        /// </summary>
        [DefaultValue("token")]
        public string authentication_method { get; set; } = "token";

        /// <summary>
        /// 开启心跳消息鉴权
        /// </summary>
        [DefaultValue(false)]
        public bool authenticate_heartbeats { get; set; }

        /// <summary>
        /// 开启建立工作连接的鉴权
        /// </summary>
        [DefaultValue(false)]
        public bool authenticate_new_work_conns {  get; set; }

        /// <summary>
        /// 鉴权使用的 token 值(客户端需要设置一样的值才能鉴权通过)
        /// </summary>
        public string token {  get; set; }

        /// <summary>
        /// oidc_issuer
        /// </summary>
        public string oidc_issuer {  get; set; }

        /// <summary>
        /// oidc_audience
        /// </summary>
        public string oidc_audience {  get; set; }

        /// <summary>
        /// oidc_skip_expiry_check
        /// </summary>
        [DefaultValue(false)]
        public bool oidc_skip_expiry_check {  get; set; }

        /// <summary>
        /// oidc_skip_issuer_check
        /// </summary>
        [DefaultValue(false)]
        public bool oidc_skip_issuer_check {  get; set; }

        /// <summary>
        /// 是否启用此模块
        /// </summary>
        [DefaultValue(true)]
        public bool enabled { get; set; }
    }

    /// <summary>
    /// 管理配置
    /// </summary>
    public class AdminConfig
    {
        /// <summary>
        /// 允许代理绑定的服务端端口,格式为 1000-2000,2001,3000-4000
        /// </summary>
        public string allow_ports {  get; set; }

        /// <summary>
        /// </summary>
        [DefaultValue(5)]
        public int max_pool_count { get; set; } = 5;

        /// <summary>
        /// 限制单个客户端最大同时存在的代理数
        /// </summary>
        public int max_ports_per_client {  get; set; }

        /// <summary>
        /// 只接受启用了 TLS 的客户端连接
        /// </summary>
        [DefaultValue(false)]
        public bool tls_only {  get; set; }

        /// <summary>
        /// 是否启用此模块
        /// </summary>
        [DefaultValue(false)]
        public bool enabled {  get; set; }
    }

    /// <summary>
    /// Dashboard 管理监控
    /// </summary>
    public class DashboardConfig
    {
        /// <summary>
        /// 启用 Dashboard 监听的本地地址
        /// </summary>
        [DefaultValue("0.0.0.0")]
        public string dashboard_addr { get; set; } = "0.0.0.0";

        /// <summary>
        /// 启用 Dashboard 监听的本地端口
        /// </summary>
        public int dashboard_port { get; set; } = 6443;

        /// <summary>
        /// HTTP BasicAuth 用户名
        /// </summary>
        public string dashboard_user { get; set; } = "admin";

        /// <summary>
        /// HTTP BasicAuth 密码
        /// </summary>
        public string dashboard_pwd { get; set; } = "admin@frps";

        /// <summary>
        /// 是否提供 Prometheus 监控接口
        /// </summary>
        [DefaultValue(false)]
        public bool enable_prometheus {  get; set; }

        /// <summary>
        /// 静态资源目录
        /// </summary>
        public string asserts_dir {  get; set; }

        /// <summary>
        /// 是否启用此模块
        /// </summary>
        [DefaultValue(true)]
        public bool enabled {  get; set; }
    }

    /// <summary>
    /// HTTP & HTTPS
    /// </summary>
    public class HttpConfig
    {
        /// <summary>
        /// 为 HTTP 类型代理监听的端口
        /// </summary>
        public int vhost_http_port {  get; set; }

        /// <summary>
        /// 为 HTTPS 类型代理监听的端口
        /// </summary>
        public int vhost_https_port {  get; set; }

        /// <summary>
        /// HTTP 类型代理在服务端的 ResponseHeader 超时时间 	
        /// </summary>
        [DefaultValue(60)]
        public int vhost_http_timeout { get; set; } = 60;

        /// <summary>
        /// 二级域名后缀
        /// </summary>
        public string subdomain_host {  get; set; }

        /// <summary>
        /// 自定义 404 错误页面地址
        /// </summary>
        public string custom_404_page {  get; set; }

        /// <summary>
        /// 为 TCPMUX 类型代理监听的端口
        /// </summary>
        public int tcpmux_httpconnect_port { get; set; }

        /// <summary>
        /// 是否启用此模块
        /// </summary>
        [DefaultValue(true)]
        public bool enabled { get; set; }
    }
}
