#UTNWO17.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/UNWO17.NWO17R1
	then
		rm ${reportdir}/UNWO17.NWO17R1
fi

if test -a /sas/whse/progrevw/UNWO17.NWO17R2
   then
        rm /sas/whse/progrevw/UNWO17.NWO17R2
fi

if test -a ${reportdir}/UNWO17.NWO17RZ
   then
        rm ${reportdir}/UNWO17.NWO17RZ
fi

# run the program

sas ${codedir}/UTNWO17.sas -log ${reportdir}/UNWO17.NWO17R1  -mautosource
