# A .NET Standard 1.4 library for the Microsoft Cognitive Services APIs.

As of now, it supports [OCR](https://westus.dev.cognitive.microsoft.com/docs/services/56f91f2d778daf23d8ec6739/operations/56f91f2e778daf14a499e1fc) processing of the [Computer Vision API v1.0](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/home).

Feel free to add support for other APIs via pull requests.

## Library usage

Invoke either of the two overloads of the `RecognizeAsync` method of the [JanLenoch.CognitiveServices.Ocr.OcrClient](https://github.com/JanLenoch/JanLenoch.CognitiveServices/blob/master/JanLenoch.CognitiveServices.Ocr/OcrClient.cs) class. The first overload is suited for submitting URIs of publicly accessible images, the second one accepts a `byte[]` array of an image.

The methods return an [OcrResponse](https://github.com/JanLenoch/JanLenoch.CognitiveServices/blob/master/JanLenoch.CognitiveServices.Ocr/Models/OcrResponse.cs) object with regions, lines and words. All hierarchically structured, with bounding boxes and coordinates.