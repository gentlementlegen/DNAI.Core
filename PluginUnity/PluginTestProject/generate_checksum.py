import hashlib

def hash_bytestr_iter(bytesiter, hasher, ashexstr=False):
    for block in bytesiter:
        hasher.update(block)
    return (hasher.hexdigest() if ashexstr else hasher.digest())

def file_as_blockiter(afile, blocksize=65536):
    print(afile)
    with afile:
        block = afile.read(blocksize)
        while len(block) > 0:
            yield block
            block = afile.read(blocksize)

fnamelst = {
    "Cntk.Composite-2.6.dll", "Cntk.Core.CSBinding-2.6.dll",
    "Cntk.Core-2.6.dll", "Cntk.Deserializers.Binary-2.6.dll",
    "Cntk.Deserializers.HTK-2.6.dll", "Cntk.Deserializers.Image-2.6.dll",
    "Cntk.Deserializers.TextFormat-2.6.dll", "Cntk.Math-2.6.dll",
    "Cntk.PerformanceProfiler-2.6.dll", "cublas64_90.dll", "cudart64_90.dll",
    "cudnn64_7.dll", "curand64_90.dll", "cusparse64_90.dll",
    "libiomp5md.dll", "mkldnn.dll", "mklml.dll", "nvml.dll",
    "opencv_world310.dll", "zip.dll", "zlib.dll"
    }

hashes = [(fname, hash_bytestr_iter(file_as_blockiter(open(fname, 'rb')), hashlib.md5()))
    for fname in fnamelst]

for h in hashes:
    with open("checksum_" + h[0][:-4] + ".md5", 'wb') as file:
        file.write(h[1])

