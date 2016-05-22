/**
 * Compresses an Image and saves it to a destination.
 * @param String $source Image file to compress.
 * @param Double $quality The quality to compress.
 * @param String $destination If set, is the specified location where to save the compressed image.
 * @return The compressed image file. 
 * @example usage : compress("Image1.png", 0.7, "ImageCompressed.jpg");
 */
function compress_image($source, $quality = 0.7, $destination = $source) {
    $info = getimagesize($source);
    if ($info['mime'] == 'image/jpeg') {
        $image = imagecreatefromjpeg($source);
        imagejpeg($image, $destination, $quality);
    } elseif ($info['mime'] == 'image/gif') { 
        $image = imagecreatefromgif($source);
        imagegif($image, $destination, $quality);
    } elseif ($info['mime'] == 'image/png') { 
        $image = imagecreatefrompng($source);
        imagepng($image, $destination, $quality);
    } else {
        throw new BadFunctionCallException("Invalid Image format");
    }
	return $destination;
}
