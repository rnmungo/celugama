import React from 'react';

// Components
import HighlightOffIcon from '@material-ui/icons/HighlightOff';
import CheckCircleOutlineIcon from '@material-ui/icons/CheckCircleOutline';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';

// Styles
import { green, red, grey } from '@material-ui/core/colors';

const PasswordMatchIcon = ({ password, toCompare }) => {
    const match = password === toCompare;
    const dontCompare = password === '';
    if (dontCompare) {
        return <LockOutlinedIcon style={{ color: grey[500] }} />;
    } else if (match) {
        return <CheckCircleOutlineIcon style={{ color: green[500] }} />
    } else {
        return <HighlightOffIcon style={{ color: red[500] }} />
    }

};

export default PasswordMatchIcon;