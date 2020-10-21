﻿using Android.Content;
using Android.Graphics;
using Camera2Forms.Camera2;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CameraPreview = Camera2Forms.CustomViews.CameraPreview;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraViewServiceRenderer))]
namespace Camera2Forms.Camera2
{
    public class CameraViewServiceRenderer : ViewRenderer<CameraPreview, CameraDroid>
	{
		private CameraDroid _camera;
        private CameraPreview _currentElement;
        private readonly Context _context;

		public CameraViewServiceRenderer(Context context) : base(context)
		{
			_context = context;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
		{
			base.OnElementChanged(e);

			_camera = new CameraDroid(Context);

            SetNativeControl(_camera);

            if (e.NewElement != null && _camera != null)
			{
                e.NewElement.CameraClick = new Command(() => TakePicture());
                _currentElement = e.NewElement;
                _camera.SetCameraOption(_currentElement.Camera);
                _camera.Photo += OnPhoto;
            }
		}

        public void TakePicture()
        {
            _camera.LockFocus();
        }

        private void OnPhoto(object sender, byte[] imgSource)
		{
            //Here you have the image byte data to do whatever you want 
            string filename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "photo.jpg");
            var fileStream = new FileStream(filename, FileMode.Create);
            var bitmap = BitmapFactory.DecodeByteArray(imgSource, 0, imgSource.Length);

            // Fazendo esta rotação, pois o '.Set(CaptureRequest.JpegOrientation...' no CameraDroid não está funcionando.
            var matrix = new Matrix();
            matrix.PostRotate(_camera.GetOrientation());
            bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, fileStream);
            fileStream.Close();

            Device.BeginInvokeOnMainThread(() =>
            {
                _currentElement?.PictureTaken();
            });   
        }

        protected override void Dispose(bool disposing)
		{
			_camera.Photo -= OnPhoto;

			base.Dispose(disposing);
		}
	}
}
