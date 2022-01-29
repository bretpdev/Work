#UTLWO97.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO97.LWO97R1
then
rm ${reportdir}/ULWO97.LWO97R1
fi
if test -a ${reportdir}/ULWO97.LWO97R2
then
rm ${reportdir}/ULWO97.LWO97R2
fi
if test -a ${reportdir}/ULWO97.LWO97R3
then
rm ${reportdir}/ULWO97.LWO97R3
fi
if test -a ${reportdir}/ULWO97.LWO97R4
then
rm ${reportdir}/ULWO97.LWO97R4
fi
if test -a ${reportdir}/ULWO97.LWO97R5
then
rm ${reportdir}/ULWO97.LWO97R5
fi
if test -a ${reportdir}/ULWO97.LWO97R6
then
rm ${reportdir}/ULWO97.LWO97R6
fi
if test -a ${reportdir}/ULWO97.LWO97RZ
then
rm ${reportdir}/ULWO97.LWO97RZ
fi

# run the program

sas ${codedir}/UTLWO97.sas -log ${reportdir}/ULWO97.LWO97R1  -mautosource
