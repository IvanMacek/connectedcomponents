Description
===========
	
	A simple application that implements Connected components labeling on images using FloodFill algorithm.
	
	For Connected componenet labeling see: http://en.wikipedia.org/wiki/Connected-component_labeling
	For FloodFill algorithm see: http://en.wikipedia.org/wiki/Flood_fill

Dependencies
============

	* .NET 3.5 SP1 Client Profile
	* AForge.NET Imaging Library (no installation required) (http://www.aforgenet.com/)

Usage
=====

	Fesb.Dip.ConnectedComponents PATH MONO_TRESHOLD BG_TRESHOLD
	
Parameters
==========

	PATH (required) 
		Path to source image used for processing.
		
	MONO_TRESHOLD (optional) 
		Number used when converting source image to monochrome image. 
		All pixel values under MONO_TRESHOLD will be black and values above will be white.
		If not provided default value of 127 will be used.

	BG_TRESHOLD (optional)
		Number used to identify background. 
		All pixel values under BG_TRESHOLD will be excluded from blob detection.
		If not provided algorithm assumes there is no background and everything is part of some blob.

Output
======
	
	Console 
		Detected blobs count will be written to console out.

	Files
		Program will output 3 different files in the same directory as source image.
		 * gray.{imageName}.png - grayscale representation od source image, where {imageName} is the name of source image file
		 * mono.{MONO_TRESHOLD}.{imageName}.png - monochromatic (black & white) representation of source image, where {MONO_TRESHOLD} is program parameter
		 * cc.{MONO_TRESHOLD}.{imageName}.png - image with color labeled detected blobs
		
Author
======

	Ivan Macek (imacek@fesb.hr)

