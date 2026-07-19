# 水准网平差程序

基于 .NET 8 与 Avalonia 12 的跨平台桌面应用，可在 Windows、macOS 和 Linux 上运行。

## 运行

```powershell
dotnet run --project kcsj.csproj
```

## 发布

以下命令生成依赖目标计算机 .NET 8 运行时的发布包：

```powershell
dotnet publish kcsj.csproj -c Release -r win-x64 --self-contained false
dotnet publish kcsj.csproj -c Release -r linux-x64 --self-contained false
dotnet publish kcsj.csproj -c Release -r osx-x64 --self-contained false
```

如需不依赖目标计算机预装 .NET，将 `--self-contained false` 改为 `--self-contained true`。

## 自动发布

推送以 `v` 开头的版本标签会触发 Gitea Actions，自动生成 Windows、Linux 和 macOS 的 x64 自包含发布包及 SHA-256 校验文件。

正式版本使用不带预发布后缀的标签：

```powershell
git tag v1.2.3
git push origin v1.2.3
```

包含 `-` 后缀的 SemVer 标签会自动创建 Pre-release：

```powershell
git tag v1.3.0-rc.1
git push origin v1.3.0-rc.1
```

## 输入格式

已知点文件每行包含点名和高程：

```text
BM1 100.0000
```

观测文件每行包含起点、终点、高差和距离：

```text
BM1 P1 1.2345 2.5
```

字段可以使用空格、Tab、英文逗号或中文逗号分隔。空行以及以 `#` 或 `//` 开头的行会被忽略。
