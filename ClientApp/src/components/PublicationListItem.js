import React, { Fragment } from 'react';

// Components
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Typography from '@material-ui/core/Typography';
import Divider from '@material-ui/core/Divider';

// Styles
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    inline: {
        display: 'inline'
    }
}))

const PublicationListItem = ({
    title,
    sellerSKU,
    variationColor,
    itemID,
    quantity,
    unitPrice
}) => {

    const classes = useStyles();

    return (
        <Fragment>
            <ListItem button>
                <ListItemText primary={<b>{`${title} - ${itemID}`}</b>} secondary={
                    <Fragment>
                        <Typography
                            component="span"
                            variant="body2"
                            className={classes.inline}
                            color="textPrimary"
                        >
                            {`SKU: ${sellerSKU ? sellerSKU : 'N/A'} | Variación: ${variationColor ? variationColor : 'N/A'}`}
                        </Typography>
                        {` - ${quantity} uni. x $ ${unitPrice}, total $ ${quantity * unitPrice}`}
                    </Fragment>
                } />
            </ListItem>
            <Divider />
        </Fragment>
    );
}

export default PublicationListItem;