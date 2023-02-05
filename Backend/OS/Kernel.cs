using System.Reflection;

namespace BOS.OS;

public class Kernel
{
    public string stdin { get; set; }
    public string stdout { get; set; }
    private FileSystem fs { get; set; }

    public Kernel()
    {
        stdin = "";
        stdout = "";
        fs = new FileSystem("C:/TestTest/");
    }

    public void STDIN_FLUSH()
    {
        stdin = "";
    }
}

public enum KernelCalls
{
    WRITE_INPUT,
    WRITE_OUTPUT,
    READ_OUTPUT,
    READ_INPUT,
    CREATE_FILE,
    DELETE_FILE,
    READ_FILE,
    WRITE_TO_FILE,
    APPEND_TO_FILE,
    SYSTEM_RUN,
    SYSTEM_OFF
}