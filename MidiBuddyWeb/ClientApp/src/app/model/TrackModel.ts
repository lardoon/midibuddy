import { IKit, IKitInstrument } from "../file-upload.service";

export interface TrackModel {
  chosenKit?: string;
  tracks: Array<Track>;
}

export interface Track {
  
  id: number;
  name?: string;
  channel: number;
  instrumentName: string;
  instrumentNumber: number;
  instrumentFamily: string;
  kitInstrumentName?: string;
  chosenKitInstruments?: {
    [key: string]: IKitInstrument
  }
  noteMap: object;
}
