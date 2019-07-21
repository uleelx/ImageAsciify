# ImageAsciify

Convert an image file to ascii text html in grayscale.

## Usage

dotnet ImageAsciify.dll <ImageFilePath> [-g][-l \<n\>][-s \<n\>][-b \<n\>]

- -g       : with HistogramEqualization (Default: false)
- -l \<n\> : max pixel size (Default: 512)
- -s \<n\> : choose convolution kernal (Default: 0)
- -b \<n\> : grayscale depth (Default: 4)
