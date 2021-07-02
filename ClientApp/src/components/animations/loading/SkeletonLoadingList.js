import React, { Fragment } from 'react';
import PropTypes from 'prop-types';

// Components
import Skeleton from '@material-ui/lab/Skeleton';
import Grid from '@material-ui/core/Grid';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import Divider from '@material-ui/core/Divider';

const SkeletonLoadingList = ({ show, items }) => {
    return show ? (
        <List>
            {items && Array(items).fill(0, 0, items).map((nr, index) => {
                return (
                    <Fragment key={index}>
                        <ListItem alignItems="flex-start">
                            <ListItemAvatar>
                                <Skeleton variant="circle" width={40} height={40} />
                            </ListItemAvatar>
                            <Grid container style={{ marginTop: 10 }}>
                                <Grid item xs={12}>
                                    <Skeleton width="50%" height={10} style={{ marginTop: 6 }} />
                                </Grid>
                                <Grid item xs={12}>
                                    <Skeleton width="60%" height={10} style={{ marginTop: 6 }} />
                                </Grid>
                            </Grid>
                            <ListItemSecondaryAction>
                                <Skeleton width={20} height={30} />
                            </ListItemSecondaryAction>
                        </ListItem>
                        <Divider variant="inset" component="li" />
                    </Fragment>
                );
            })}
        </List>
    ) : null;
}

SkeletonLoadingList.propTypes = {
    show: PropTypes.bool,
    items: PropTypes.number
}

SkeletonLoadingList.defaultProps = {
    show: false,
    items: 3
}

export default SkeletonLoadingList;