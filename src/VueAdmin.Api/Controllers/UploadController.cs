using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Serilog;
using System.Drawing;
using System.Text.RegularExpressions;
using VueAdmin.Api.Dtos;
using VueAdmin.Core.Enums;
using VueAdmin.Data;
using VueAdmin.Repository;

namespace VueAdmin.Api.Controllers
{
    [Route("api/upload")]
    [ApiController]
    [Authorize]
    public class UploadController : ApiBaseController
    {
        private IWebHostEnvironment _hostingEnv;
        private readonly IConfiguration _config;
        private IMapper _mapper;
        private UploadFileDto FileDto;
        private UploadFileDto ImageDto;
        private string serverHost;
        private IRepository<PictureGallery> _pictureRepository;
        private IRepository<Attachments> _attachRepository;


        public UploadController(
            IWebHostEnvironment hostingEnv,
            IConfiguration configuration,
            IMapper mapper,
            IRepository<PictureGallery> pictureRepository,
            IRepository<Attachments> attachRepository
            )
        {
            _hostingEnv = hostingEnv;
            _config = configuration;
            _mapper = mapper;

            serverHost = _config["App:ServerHost"];

            ImageDto = new UploadFileDto
            {
                FileType = (int)FileType.Image,
                Path = $"{_config["UploadConfig:Image:Path"]}/{DateTime.Now:yyyyMM}/{DateTime.Now:dd}/",
                ExtendedName = _config["UploadConfig:Image:ExtName"].Split('|'),
                MaxSize = Convert.ToInt64(_config["UploadConfig:Image:Size"])
            };

            FileDto = new UploadFileDto
            {
                FileType = (int)FileType.File,
                Path = $"{_config["UploadConfig:File:Path"]}/{DateTime.Now:yyyyMM}/{DateTime.Now:dd}/",
                ExtendedName = _config["UploadConfig:File:ExtName"].Split('|'),
                MaxSize = Convert.ToInt64(_config["UploadConfig:File:Size"])
            };

            _pictureRepository = pictureRepository;
            _attachRepository = attachRepository;
        }

        #region 上传图片       

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file">文件</param>       
        /// <returns></returns>
        [HttpPost]
        [Route("image")]
        public async Task<ResultDto<UploadOutputDto>> UploadImage(IFormFile file)
        {
            var result = new ResultDto<UploadOutputDto>();
            if (file == null)
            {
                result.Msg = "请选择上传文件";
                return result;
            }
            var oldFileName = file.FileName;//原文件名
            var extName = Path.GetExtension(file.FileName).ToLower(); //文件扩展名
            var size = file.Length;

            if (!ImageDto.ExtendedName.Contains(extName))    //"图片格式";
            {
                result.Msg = "文件格式错误";
                return result;
            }

            if (size > ImageDto.MaxSize * 1024 * 1024)    //"图片大小";
            {
                result.Msg = $"大小不能超过{ImageDto.MaxSize}M";
                return result;
            }

            var fileName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名
            var fileUrl = ImageDto.Path + fileName;//返回文件的相对路径

            using (var stream = file.OpenReadStream())
            {
                var isOk = await Save(stream, ImageDto.Path, fileName);
                if (isOk)
                {
                    var image = Image.FromStream(stream);

                    var dto = new PictureGalleryDto()
                    {
                        ImageName = oldFileName,
                        Url = serverHost + "/" + fileUrl,
                        ExtensionName = extName,
                        Size = size,
                        Width = image.Width,
                        Height = image.Height
                    };

                    var model = _mapper.Map<PictureGallery>(dto);
                    model.CreateBy = LoginUser.UserName;
                    if ((await _pictureRepository.AddAsync(model)).Id > 0)
                    {
                        var data = new UploadOutputDto()
                        {
                            FileName = dto.ImageName,
                            Url = dto.Url,
                            ExtensionName = dto.ExtensionName
                        };
                        result.SetData(data);
                    }
                    return result;
                }
                else
                {
                    result.Error("上传失败");
                    return result;
                }
            }
        }

        /// <summary>
        /// 上传base64格式图片
        /// </summary>
        /// <param name="input"></param>   
        /// <returns></returns>
        [HttpPost]
        [Route("base64-image")]
        public async Task<ResultDto<UploadOutputDto>> UploadImageFromBase64([FromBody] ImageBase64Dto input)
        {
            var result = new ResultDto<UploadOutputDto>();

            if (input.Base64.Length == 0)
            {
                result.Msg = "请选择上传文件";
                return result;
            }

            Regex reg = new Regex(@"(?<=\/)[^\/]+(?=\;)");
            var extName = "." + reg.Match(input.Base64).ToString(); //文件扩展名

            if (!ImageDto.ExtendedName.Contains(extName))    //"图片格式";
            {
                result.Msg = "图片格式错误";
                return result;
            }

            var size = Utils.ImageSize(input.Base64);
            if (size > ImageDto.MaxSize * 1024 * 1024)    //"图片大小";
            {
                result.Msg = $"大小不能超过{ImageDto.MaxSize}M";
                return result;

            }

            var fileName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名            
            var fileUrl = ImageDto.Path + fileName;//返回文件的相对路径

            using (var stream = Utils.ConvertBase64ToStream(input.Base64))
            {
                var isOk = await Save(stream, ImageDto.Path, fileName);
                if (isOk)
                {
                    var image = Image.FromStream(stream);

                    var dto = new PictureGalleryDto()
                    {
                        ImageName = input.FileName,
                        Url = serverHost + "/" + fileUrl,
                        ExtensionName = extName,
                        Size = size,
                        Width = image.Width,
                        Height = image.Height
                    };

                    var model = _mapper.Map<PictureGallery>(dto);
                    model.CreateBy = LoginUser.UserName;
                    if ((await _pictureRepository.AddAsync(model)).Id > 0)
                    {
                        var data = new UploadOutputDto()
                        {
                            FileName = dto.ImageName,
                            Url = dto.Url,
                            ExtensionName = dto.ExtensionName
                        };
                        result.SetData(data);
                    }

                    return result;
                }
                else
                {
                    result.Error("上传失败");
                    return result;
                }
            }
        }

        #endregion


        #region  上传附件 本地
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("file")]
        public async Task<ResultDto<Dictionary<string, object>>> UploadFile(IFormFile file, string fileName, string tempDirectory, int index, int total, int totalSize = 0)
        {
            var result = new ResultDto<Dictionary<string, object>>();

            if (file == null)
            {
                result.Error("请选择上传文件");
                return result;
            }
            var oldFileName = fileName;// file.FileName;//原文件名
            var extName = Path.GetExtension(fileName).ToLower(); //文件扩展名
            var size = file.Length;

            if (!FileDto.ExtendedName.Contains(extName))    //"图片格式";
            {
                result.Error("文件格式错误");
                return result;
            }

            if (totalSize > FileDto.MaxSize * 1024 * 1024)    //"图片大小";
            {
                result.Error($"大小不能超过{FileDto.MaxSize}M");
                return result;
            }

            var saveName = DateTime.Now.Ticks.ToString() + extName;//保存的文件名
            string tmp = Path.Combine(FileDto.Path, tempDirectory) + "/";//临时保存分块的目录
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    if (await Save(stream, tmp, index.ToString()))
                    {
                        var uploadpath = FileDto.Path + saveName;//返回文件的相对路径
                        bool mergeOk = false;
                        if (total == index)
                        {
                            mergeOk = await FileMerge(tmp, FileDto.Path, saveName);
                            if (mergeOk)
                            {
                                var dto = new AttachmentsDto()
                                {
                                    FileName = oldFileName,
                                    Url = serverHost + "/" + uploadpath,
                                    ExtensionName = extName,
                                    Size = size
                                };

                                var model = _mapper.Map<Attachments>(dto);
                                model.CreateBy = LoginUser.UserName;
                                await _attachRepository.AddAsync(model);
                            }
                        }

                        var dic = new Dictionary<string, object>();
                        dic.Add("number", index);
                        dic.Add("mergeOk", mergeOk);
                        dic.Add("filename", saveName);

                        result.SetData(dic);
                        return result;
                    }
                    else
                    {
                        result.Error("上传失败");
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Directory.Delete(_hostingEnv.WebRootPath + tmp);//删除文件夹
                Log.Error($"上传异常:{ex.Message}");
                result.Error("上传失败");
                return result;
            }

        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="tmpDirectory">临时上传目录</param>        
        /// <param name="path">上传目录</param>
        /// <param name="saveFileName">保存之后新文件名</param>
        /// <returns></returns>
        private async Task<bool> FileMerge(string tmpDirectory, string path, string saveFileName)
        {
            try
            {
                var tmpPath = _hostingEnv.WebRootPath + tmpDirectory;//获得临时目录下面的所有文件
                var serverPath = Path.Combine(_hostingEnv.WebRootPath + path, saveFileName);//最终保存的文件路径
                var files = Directory.GetFiles(tmpPath);

                using (var fs = new FileStream(serverPath, FileMode.Create))
                {
                    foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))
                    {
                        var bytes = System.IO.File.ReadAllBytes(part);
                        await fs.WriteAsync(bytes, 0, bytes.Length);
                        bytes = null;
                        System.IO.File.Delete(part);//删除分块
                    }
                    fs.Close();

                    Directory.Delete(tmpPath);//删除临时目录
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("上传合并文件异常", ex);
                return false;
            }

        }

        #endregion


        /// <summary>
        /// 文件保存到本地
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="path"></param>
        /// <param name="saveName"></param>
        /// <returns></returns>
        private async Task<bool> Save(Stream stream, string path, string saveName)
        {
            try
            {
                var serverPath = _hostingEnv.WebRootPath + "/" + path;
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                await Task.Run(async () =>
                {
                    using (FileStream fs = new FileStream(serverPath + saveName, FileMode.Create))
                    {
                        stream.Position = 0;
                        await stream.CopyToAsync(fs);
                        fs.Close();
                    }
                });
                return true;

            }
            catch (Exception ex)
            {
                Log.Error("上传异常", ex);
                return false;
            }
        }
    }
}
