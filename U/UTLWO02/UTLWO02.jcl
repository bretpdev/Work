#UTLWO02.jcl  Loan Sale (rewrite) Trigger and Reporting
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO02.LWO02R1
   then
        rm ${reportdir}/ULWO02.LWO02R1
fi
if test -a ${reportdir}/ULWO02.LWO02R2
   then
        rm ${reportdir}/ULWO02.LWO02R2
fi
if test -a ${reportdir}/ULWO02.LWO02R11
   then
        rm ${reportdir}/ULWO02.LWO02R11
fi
if test -a ${reportdir}/ULWO02.LWO02R30
   then
        rm ${reportdir}/ULWO02.LWO02R30
fi
if test -a ${reportdir}/ULWO02.LWO02R31
   then
        rm ${reportdir}/ULWO02.LWO02R31
fi
if test -a ${reportdir}/ULWO02.LWO02R32
   then
        rm ${reportdir}/ULWO02.LWO02R32
fi
if test -a ${reportdir}/ULWO02.LWO02R33
   then
        rm ${reportdir}/ULWO02.LWO02R33
fi
if test -a ${reportdir}/ULWO02.LWO02R50
   then
        rm ${reportdir}/ULWO02.LWO02R50
fi
if test -a ${reportdir}/ULWO02.LWO02R52
   then
        rm ${reportdir}/ULWO02.LWO02R52
fi
if test -a ${reportdir}/ULWO02.LWO02R54
   then
        rm ${reportdir}/ULWO02.LWO02R54
fi
if test -a ${reportdir}/ULWO02.LWO02R70
   then
        rm ${reportdir}/ULWO02.LWO02R70
fi
if test -a ${reportdir}/ULWO02.LWO02R75
   then
        rm ${reportdir}/ULWO02.LWO02R75
fi
if test -a ${reportdir}/ULWO02.LWO02R90
   then
        rm ${reportdir}/ULWO02.LWO02R90
fi

# run the program

sas ${codedir}/UTLWO02.sas -log ${reportdir}/ULWO02.LWO02R1  -mautosource

RC=$?
if [ $RC = 99 ]
 then
   echo "There were no loan sales for todays processing"
   exit 0
fi

if [ $RC -gt 1 ] && [ $RC -ne 99 ]
 then
   echo "Job failed due to invalid SAS processing"
   exit 8
fi

# set full group/non-group permissions for lock-down file and trigger file

chmod 666 /sas/whse/olrp_lookup_directory/utlwo02.sas7bdat

if test -a ${reportdir}/ULWO02.LWO02R2
   then
	chmod 666 ${reportdir}/ULWO02.LWO02R2
fi


##########################################################################
# FTP trigger file to mainframe 
#
# Set Environment variables
. /sas/whse/copy_lib/pathprofile.dblgs

jobname=UTLWO02.jcl
filename=ULWO02.LWO02R2
FILE=UTLWO02.FTP03
filelog=UTLWO02.FT3.log

DTS=`date +d%Y%j.t%H%M%S`
  echo  "DTS=  $DTS"
DIR=/sas/whse
  echo  " DIR =  $DIR "
USER=`cat /sas/whse/copy_lib/ftpmbox | cut -c1-7` 
  echo  " USER =  $USER "
PASS=`cat /sas/whse/copy_lib/ftpmbox | cut -c9-15` 
  echo  " PASS =  $PASS "
ERRLOG=$DIR/spool/logs/errlog
  echo  "DTS=  $DTS"
LOG=$DIR/spool/logs/log
  echo  " LOG= $LOG"

# Exit gracefully if the file is empty or does not exist
if [ ! -f "${reportdir}/${filename}" -o ! -s "${reportdir}/${filename}" ]
 then
   echo "${reportdir}/${filename} is empty or does not exist ... FTP terminated"
   exit 0
fi

# copy and rename file for the FTP process
cp "${reportdir}/${filename}" "${DIR}/${FILE}"

if [ -f "${DIR}/${FILE}" ]
 then
   FILE2="${FILE}.${DTS}"
   cp "${DIR}/${FILE}" "${DIR}/progrevw/${FILE2}"
 else
   echo "date" >> $ERRLOG
   echo "${DIR}/${FILE} does not exist" >> $ERRLOG
fi

# check status of copy
if [ "$?" = 1 ]
 then
   echo `date` >> $ERRLOG
   echo "Copy of "${DIR}/${FILE}" to ${DIR}/progrevw/${FILE2} failed" >> $ERRLOG
   exit 1
else
   echo `date` >> $LOG
   echo "Copy of "${DIR}/${FILE}" to ${DIR}/progrevw/${FILE2} was successful" >> $LOG
fi


echo "${FILE2}"

# Logon to HERA & send the file
ftp -nv hera <<EOF > ${DIR}/spool/logs/${filelog}
user "${USER}" "${PASS}"
macdef init

quote site filetype=seq
cd 'ftp.ftp0020'
lcd "${DIR}/progrevw"
put "$FILE2"
EOF

grep -i 'transfer completed successfully' ${DIR}/spool/logs/${filelog}

if [ "$?" -gt 0 ]
   then
        echo `date` >> $ERRLOG
        echo "FTP failed, files not removed" >> $ERRLOG
        echo "FTP failed, files not removed"
        exit 1
   else
        echo `date` >> $LOG
        echo "FTP successful, files removed" >> $LOG
        echo "FTP successful, files removed"
        rm "$DIR/progrevw/$FILE2"
        rm "${DIR}/${FILE}"
fi

RC=$?

echo "${jobname} ${region} Exit status = $RC"

