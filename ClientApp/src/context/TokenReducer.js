export const SET_IS_AUTH = 'SET_IS_AUTH';

export const initialState = {
    isAuth: false
}

export const TokenReducer = (state, action) => {
    switch (action.type) {
        case SET_IS_AUTH:
            return {
                ...state,
                isAuth: action.isAuth
            };
        default:
            return state;
    }
}