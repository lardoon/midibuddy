import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { HttpClient, HttpEvent, HttpRequest, HttpParams, HttpResponse, HttpEventType } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  constructor(private http: HttpClient) { }

  getDrumKits(): Observable<IResult> {
    let url = 'api/GetDrumkits';
    
    let params = new HttpParams();

    const options = {
      params: params,
      reportProgress: true,
    };
    const req = new HttpRequest('GET', url, options);
    return this.http.request(req).pipe(map(event => {
      if (event.type == HttpEventType.UploadProgress) {
        const percentDone = Math.round(100 * event.loaded / event.total);
        return {
          progress: percentDone,
          data: null
        }

      } else if (event instanceof HttpResponse) {
        return {
          progress: 100,
          data: event.body
        }
      }
    }));
   }

  postFile(fileToUpload: File): Observable<IResult> {
    let url = 'api/MidiFileUpload';
    let formData = new FormData();
    formData.append('upload', fileToUpload);

    let params = new HttpParams();

    const options = {
      params: params,
      reportProgress: true,
    };

    const req = new HttpRequest('POST', url, formData, options);
    return this.http.request(req).pipe(map(event => {
      if (event.type == HttpEventType.UploadProgress) {
        const percentDone = Math.round(100 * event.loaded / event.total);
        return {
          progress: percentDone,
          data: null
        }

      } else if (event instanceof HttpResponse) {
        return {
          progress: 100,
          data: event.body
        }
      }
    }));

  }



  handleError(e: Error) {
    console.error(e);
  }
}

export interface IResult {
  progress: number;
  data: Object;
}
