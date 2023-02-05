using Microsoft.AspNetCore.Mvc;
using BOS.OS;
using System.IO;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class KernelController : ControllerBase
{
    private Kernel k = new Kernel();

    [HttpPost]
    [Route("write_to_file")]
    public async Task WriteFile(byte[] val, FLE file)
    {
        file.contents = val;
        await file.Write();
    }

    [HttpGet]
    [Route("read_from_file")]
    public async Task<byte[]> ReadFile(FLE file)
    {
        return await file.Read();
    }

    [HttpGet]
    [Route("read_input")]
    public async Task<string> ReadInput()
    {
        while(k.stdin != "")
        {
            continue;
        }

        string ret = k.stdin;
        k.STDIN_FLUSH();

        return ret;
    }

    [HttpPost]
    [Route("write_input")]
    public async Task WriteInput(string input)
    {
        k.stdin = input;
    }
}
