import { createFeature } from "@ngrx/store"
import { authReducer } from "./reducers"

export const SECURITY_FEATURE_NAME = 'SECURITY';

export const SecurityFeature = createFeature({
    name: SECURITY_FEATURE_NAME,
    reducer: authReducer
});

