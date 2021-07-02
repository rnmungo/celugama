import React from 'react';
import passwordStrength from 'check-password-strength';

// Components
import SentimentVerySatisfiedIcon from '@material-ui/icons/SentimentVerySatisfied';
import SentimentVeryDissatisfiedIcon from '@material-ui/icons/SentimentVeryDissatisfied';
import SentimentSatisfiedIcon from '@material-ui/icons/SentimentSatisfied';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';

// Styles
import { green, orange, red, grey } from '@material-ui/core/colors';

const SecureIcon = security => {
    switch (security) {
        case 'Weak':
            return <SentimentVeryDissatisfiedIcon style={{ color: red[500] }} />;
        case 'Medium':
            return <SentimentSatisfiedIcon style={{ color: orange[500] }} />;
        case 'Strong':
            return <SentimentVerySatisfiedIcon style={{ color: green[500] }} />;
        default:
            return <LockOutlinedIcon style={{ color: grey[500] }} />;
    }
}


const PasswordSecureIcon = ({ password }) => {
    const security = password ? passwordStrength(password).value : '';
    return SecureIcon(security);

};

export default PasswordSecureIcon;